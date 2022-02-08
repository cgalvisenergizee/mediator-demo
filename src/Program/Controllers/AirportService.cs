using MediatR;
using Microsoft.Extensions.Logging;
using Program.Controllers.ControlTower.Commands;
using Program.Data;
using Program.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Program.Controllers
{
    public interface IAirportService
    {
        void SimulateLandings();
    }

    public class AirportService : IAirportService
    {
        private static readonly ConsoleColor DefaultColor = ConsoleColor.Gray;
        private readonly ILogger<AirportService> _logger;
        private readonly IMediator _controlTower;
        private readonly IRepository<Airport> _airportRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public AirportService(ILoggerFactory loggerFactory, IMediator mediator,
            IRepository<Airport> airportRepository, IRepository<Vehicle> vehicleRepository)
        {
            _logger = loggerFactory.CreateLogger<AirportService>();
            _controlTower = mediator;
            _airportRepository = airportRepository;
            _vehicleRepository = vehicleRepository;
        }

        #region General functions

        /// <summary>
        /// Simulate air vehicle landings
        /// </summary>
        public void SimulateLandings()
        {
            _logger.LogInformation($"Generate data ...");

            // Generate database
            InitializeDb();

            // Print airport data
            var airport = _airportRepository
                .GetAll()
                .FirstOrDefault();
            PrintAirportData(airport);

            var vehicles = _vehicleRepository.GetAll();

            // Print vehicles data
            PrintVehiclesData(vehicles);

            // Simulate landings
            SimulateLandings(vehicles).Wait();
        }

        /// <summary>
        /// Simulate air vehicle landings (asynchronous simulator)
        /// </summary>
        public async Task SimulateLandings(List<Vehicle> vehicles)
        {
            Console.WriteLine("Simulate landings ...\n");

            List<Task<bool>> tasks = new();
            foreach (var vehicle in vehicles)
                tasks.Add(SimulateLandingAsync(vehicle));

            await Task.WhenAll(tasks);
            Console.WriteLine();
        }

        /// <summary>
        /// Simulate air vehicle landing
        /// </summary>
        /// <param name="vehicle">Air vehicle data</param>
        /// <returns>True when the process has finished</returns>
        private async Task<bool> SimulateLandingAsync(Vehicle vehicle)
        {
            // Wait time with the control tower
            var rand = new Random();
            await Task.Delay(rand.Next(2000, 4000));

            var enabledTrack = await GetEnabledLandingTrack();

            if (enabledTrack != null)
            {
                Console.WriteLine($"Vehicle [{vehicle.Id}] has landed in track [{enabledTrack.Name}]");
                await UpdateLandingTrack(enabledTrack);
                Console.WriteLine($"Vehicle [{vehicle.Id}] has released the track [{enabledTrack.Name}]");
                return true;
            }
            else
            {
                Console.WriteLine($"Vehicle [{vehicle.Id}] does not have an available track");
                return await SimulateLandingAsync(vehicle);
            }
        }

        /// <summary>
        /// Get enabled landing track (with MediatR)
        /// </summary>
        /// <returns>Enabled landing track</returns>
        public async Task<LandingTrack> GetEnabledLandingTrack()
        {
            return await _controlTower.Send(new CheckLandingDataCommand());
        }

        /// <summary>
        /// Update landing track status (with MediatR)
        /// </summary>
        /// <returns>True when the process has finished</returns>
        public async Task<bool> UpdateLandingTrack(LandingTrack track)
        {
            return await _controlTower.Send(new UpdateLandingDataCommand()
            {
                LandingTrack = track,
            });
        }

        #endregion

        #region Initialize database

        /// <summary>
        /// Initialize airport data
        /// </summary>
        private void InitializeDb()
        {
            // Generate fake airport
            GenerateAirport();

            // Generate fake random vehicles
            GenerateVehicles();
        }

        /// <summary>
        /// Generate airport data
        /// </summary>
        /// <returns>Fake airport data</returns>
        private Airport GenerateAirport()
        {
            Airport airport = new()
            {
                Name = "Miami International airport"
            };

            // Add airport landing tracks
            int landingTracks = 4;
            for (int i = 0; i < landingTracks; i++)
            {
                airport.LandingTracks.Add(new()
                {
                    Name = $"T{i + 1}",
                });
            }

            // Save data
            _airportRepository.Add(airport);

            return airport;
        }

        /// <summary>
        /// Generate air vehicles data
        /// </summary>
        /// <returns>Fake air vehicles list</returns>
        private List<Vehicle> GenerateVehicles()
        {
            var rand = new Random();
            int totalHelicopters = rand.Next(1, 3);
            int totalPlanes = rand.Next(4, 8);

            List<Vehicle> vehicles = new();

            // Add helicopters
            for (int i = 0; i < totalHelicopters; i++)
                vehicles.Add(new()
                {
                    Id = $"H{i + 1}",
                    Passengers = rand.Next(2, 6),
                    Type = VehicleType.Helicopter
                });

            // Add planes
            for (int i = 0; i < totalPlanes; i++)
                vehicles.Add(new()
                {
                    Id = $"P{i + 1}",
                    Passengers = rand.Next(18, 80),
                    Type = VehicleType.Plane
                });

            // Save data
            _vehicleRepository.AddRange(vehicles);

            return vehicles;
        }

        #endregion

        #region Print functions

        /// <summary>
        /// Print airport data with format
        /// </summary>
        /// <param name="airport">Airport data</param>
        private static void PrintAirportData(Airport airport)
        {
            Dictionary<string, string> printData = new()
            {
                { "Airport", airport.Name },
                { "- Landing tracks", $"{airport.LandingTracks.Count}" },
            };

            Console.WriteLine();
            
            foreach (var data in printData)
                PrintKeyValueData(data.Key, data.Value);

            Console.WriteLine();
        }

        /// <summary>
        /// Print air vehicles data with format
        /// </summary>
        /// <param name="vehicles">Air vehicles list</param>
        private static void PrintVehiclesData(List<Vehicle> vehicles)
        {
            Dictionary<string, string> printData;

            foreach (var vehicle in vehicles)
            {
                printData = new()
                {
                    { "Vehicle", vehicle.Id },
                    { "- type", $"{vehicle.Type}" },
                    { "- passengers", $"{vehicle.Passengers}" },
                };

                foreach (var data in printData)
                    PrintKeyValueData(data.Key, data.Value);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Print key - value data
        /// </summary>
        /// <param name="key">Key data</param>
        /// <param name="value">Value data</param>
        private static void PrintKeyValueData(string key, string value)
        {
            if (key.StartsWith("-"))
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Green;

            Console.Write($"{key}: ");
            Console.ForegroundColor = DefaultColor;
            Console.WriteLine(value);
        }

        #endregion
    }
}
