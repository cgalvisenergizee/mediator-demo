using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Program.Controllers;
using Program.Data;
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
                .AddSingleton(typeof(IRepository<>), typeof(Repository<>))
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();
            logger.LogInformation("Starting application");

            // Execute program
            var service = serviceProvider.GetService<IAirportService>();
            service.SimulateLandings();

            logger.LogInformation("All done!");
        }
    }
}
