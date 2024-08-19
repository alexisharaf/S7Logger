using Microsoft.Extensions.Hosting.Systemd;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using Sharp7;


namespace S7Logger
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;


        private S7Client s7Client;

        internal AppConfig appConfig;

        public Worker(ILogger<Worker> logger, IHostLifetime lifetime)
        {
            _logger = logger;

            _logger.LogInformation("IsSystemd: {isSystemd}", lifetime.GetType() == typeof(SystemdLifetime));
            _logger.LogInformation("IHostLifetime: {hostLifetime}", lifetime.GetType());

            s7Client = new S7Client();

            appConfig = new AppConfig(_logger);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                int error, readingerror;

                int bufferSize = 4;


                if ((error = s7Client.ConnectTo(appConfig.plc.plc_ip, appConfig.plc.plc_rack, appConfig.plc.plc_slot)) == 0)
                {
                    for (int i = 0; i < appConfig.paramList.Count; i++)
                    {
                        if (appConfig.paramList[i].prameter_type.Trim() == "bool")
                        {
                            bufferSize = 1;
                        }
                        if (appConfig.paramList[i].prameter_type.Trim() == "int2")
                        {
                            bufferSize = 2;
                        }
                        if (appConfig.paramList[i].prameter_type.Trim() == "int4")
                        {
                            bufferSize = 4;
                        }
                        if (appConfig.paramList[i].prameter_type.Trim() == "int8")
                        {
                            bufferSize = 8;
                        }
                        if (appConfig.paramList[i].prameter_type.Trim() == "float4")
                        {
                            bufferSize = 4;
                        }
                        if (appConfig.paramList[i].prameter_type.Trim() == "float8")
                        {
                            bufferSize = 8;
                        }


                        byte[] Buffer = new byte[bufferSize];

                        readingerror = s7Client.ReadArea(S7Area.DB, appConfig.paramList[i].db, appConfig.paramList[i].offset, bufferSize, S7WordLength.Byte, Buffer);

                        if (appConfig.paramList[i].prameter_type.Trim() == "bool")
                        {
                            appConfig.paramList[i].Value = Buffer.GetBitAt(0, appConfig.paramList[i].bitnumber);
                        }

                        if (appConfig.paramList[i].prameter_type.Trim() == "int2")
                        {
                            appConfig.paramList[i].Value = Buffer.GetIntAt(0);
                        }
                        if (appConfig.paramList[i].prameter_type.Trim() == "int4")
                        {
                            appConfig.paramList[i].Value = Buffer.GetDIntAt(0);
                        }
                        if (appConfig.paramList[i].prameter_type.Trim() == "int8")
                        {
                            appConfig.paramList[i].Value = Buffer.GetLIntAt(0);
                        }
                        if (appConfig.paramList[i].prameter_type.Trim() == "float4")
                        {
                            appConfig.paramList[i].Value = Buffer.GetRealAt(0);
                        }
                        if (appConfig.paramList[i].prameter_type.Trim() == "float8")
                        {
                            appConfig.paramList[i].Value = Buffer.GetLRealAt(0);
                        }

                    }



                    try
                    {

                        using (NpgsqlConnection connection = new NpgsqlConnection(appConfig.databaseConnectionString))
                        {
                            connection.Open();


                            string sqlCommandParamNumerataor = " ($1";

                            for (int i = 1; i <= appConfig.paramList.Count; i++)
                            {
                                sqlCommandParamNumerataor += ", $" + (i + 1).ToString();
                            }

                            sqlCommandParamNumerataor += " )";

                            string commandstring = "INSERT INTO " + appConfig.plc.plc_name + "  VALUES " + sqlCommandParamNumerataor;

                            NpgsqlCommand command = new NpgsqlCommand(commandstring, connection)
                            {
                                // CommandType = System.Data.CommandType.TableDirect

                            };

                            NpgsqlParameter plcDt = new NpgsqlParameter()
                            {
                                //ParameterName = "@"+ appConfig.paramList[i].prameter_name,
                                DataTypeName = "timestamp without time zone",
                                Value = DateTime.Now
                            };
                            command.Parameters.Add(plcDt);

                            for (int i = 0; i < appConfig.paramList.Count; i++)
                            {
                                NpgsqlParameter plcP = new NpgsqlParameter()
                                {
                                    //ParameterName = "@"+ appConfig.paramList[i].prameter_name,
                                    DataTypeName = appConfig.paramList[i].prameter_type,
                                    Value = appConfig.paramList[i].Value
                                };
                                command.Parameters.Add(plcP);
                            }


                            command.ExecuteNonQuery();

                            //var reader = command.ExecuteReader();

                            //if (reader.Read() != false)
                            //    reqStatus = reader.GetInt32(0);


                            connection.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);

                    }
                    finally
                    {


                    }
                }
                else
                {
                    _logger.LogError("Can not connect plc.");
                }
                await Task.Delay(appConfig.delayTime, stoppingToken);
            }
        }
    }
}
