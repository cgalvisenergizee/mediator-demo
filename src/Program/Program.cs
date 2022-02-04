using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Program.Controllers;
using System.Reflection;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup Dependency Injection
            var serviceProvider = new ServiceCollection()
                .AddLogging(opt => opt.AddConsole())
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddSingleton<IAirportService, AirportService>()
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogDebug("Starting application");

            // Execute program
            var service = serviceProvider.GetService<IAirportService>();
            service.SimulateLandings();

            logger.LogDebug("All done!");
        }
    }
}
