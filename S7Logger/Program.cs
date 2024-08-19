using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.Hosting.Systemd;


namespace S7Logger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var builder = Host.CreateApplicationBuilder(args);
            //builder.Services.AddHostedService<Worker>();

            var builder = Host.CreateDefaultBuilder(args).UseSystemd().ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();
            });


            var host = builder.Build();
            host.Run();
        }
    }
}