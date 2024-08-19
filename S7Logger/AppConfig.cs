
using Npgsql;


namespace S7Logger
{
    internal class AppConfig
    {
        public string databaseConnectionString;

        public TimeSpan delayTime;

        public TimerType timer;
        public PlcType plc;

        public List<ParameterType> paramList;


        public ILogger<Worker> logger;

        public AppConfig(ILogger<Worker> log)
        {

            logger = log;

            //timer = new TimerType();

            //plc = new PlcType();

            //paramList = new List<ParameterType>();

            var configuration = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

            databaseConnectionString = configuration.GetConnectionString("DbConnectionString");
            logger.LogInformation("Connection string is: {connectionString}", databaseConnectionString);

            LoadTimer(configuration);
            logger.LogInformation("Delay time is: {delayTime}", delayTime);

            LoadPlc(configuration);
            logger.LogInformation("Plc: plc_name = {plc_name}, plc_ip = {plc_ip}, plc_rack = {plc_rack}, plc_slot = {plc_slot}", plc.plc_name, plc.plc_ip, plc.plc_rack, plc.plc_slot);

            LoadParameters(configuration);
            logger.LogInformation("Parameters number: " + paramList.Count);


            //Type myType1 = Type.GetType("System.Int32");

            //var item = Activator.CreateInstance(Type.GetType("System.Int32"));



            initDbTable();

        }

        private void LoadTimer(IConfigurationRoot configuration)
        {

            timer = new TimerType();
            var timersection = configuration.GetSection("Timer");


            //timer = JsonSerializer.Deserialize<TimerType>(timersection);

            timer.hour = timersection.GetValue<int>("hour");
            timer.minute = timersection.GetValue<int>("minute");
            timer.second = timersection.GetValue<int>("second");
            timer.millisecond = timersection.GetValue<int>("millisecond");

            delayTime = new TimeSpan(0, timer.hour, timer.minute, timer.second, timer.millisecond);

        }

        private void LoadPlc(IConfigurationRoot configuration)
        {
            plc = new PlcType();

            var plcsection = configuration.GetSection("Plc");

            plc.plc_name = plcsection.GetValue<string>("plc_name");
            plc.plc_ip = plcsection.GetValue<string>("plc_ip");
            plc.plc_rack = plcsection.GetValue<int>("plc_rack");
            plc.plc_slot = plcsection.GetValue<int>("plc_slot");
        }

        private void LoadParameters(IConfigurationRoot configuration)
        {
            paramList = new List<ParameterType>();

            var paramssection = configuration.GetSection("Parameters");

            foreach (IConfigurationSection child in paramssection.GetChildren())
            {
                ParameterType pt = new ParameterType();

                pt.prameter_name = child.GetValue<string>("parameter_name");
                pt.prameter_type = child.GetValue<string>("parameter_type");
                pt.db = child.GetValue<int>("db");
                pt.offset = child.GetValue<int>("offset");
                pt.bitnumber = child.GetValue<int>("bitnumber");

                paramList.Add(pt);
            }



        }

        private void initDbTable()
        {
            NpgsqlConnection connection = new NpgsqlConnection(databaseConnectionString);

            connection.Open();

            string createTableCmdString = "CREATE TABLE IF NOT EXISTS " + plc.plc_name.ToString().Trim() + "(time_stamp timestamp without time zone NOT NULL DEFAULT now())";

            var crateTableCmd = new NpgsqlCommand(createTableCmdString, connection);

            crateTableCmd.ExecuteNonQuery();



            for (int i = 0; i < paramList.Count; i++)
            {
                string addColumnCmdString = "ALTER TABLE IF EXISTS " + plc.plc_name + " ADD COLUMN IF NOT EXISTS " + paramList[i].prameter_name.Trim() + " " + paramList[i].prameter_type.Trim();
                var addColumnCmd = new NpgsqlCommand(addColumnCmdString, connection);
                addColumnCmd.ExecuteNonQuery();
            }


        }

    }
}
