using MediatR;
using Microsoft.Extensions.Logging;
using Program.Controllers.Vehicles.Commands;
using Program.Data;
using Program.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Program.Controllers
{
    public interface IAirportService
    {
        void SimulateLandings();
    }

    public class AirportService : IAirportService
    {
        private readonly ILogger<AirportService> _logger;
        private readonly IMediator _mediator;
        private readonly IRepository<Airport> _airportRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public AirportService(ILoggerFactory loggerFactory, IMediator mediator,
            IRepository<Airport> airportRepository, IRepository<Vehicle> vehicleRepository)
        {
            _logger = loggerFactory.CreateLogger<AirportService>();
            _mediator = mediator;
            _airportRepository = airportRepository;
            _vehicleRepository = vehicleRepository;
        }

        #region General functions

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

            _logger.LogInformation($"Simulate landings ...");

            SimulateLandings(vehicles);
        }

        public void SimulateLandings(List<Vehicle> vehicles)
        {
            foreach (var vehicle in vehicles)
            {
                SimulateLanding(vehicle);
            }
        }

        public void SimulateLanding(Vehicle vehicle)
        {
            var enabledTrack = GetEnabledLandingTrack();

            if (enabledTrack != null)
            {
                _logger.LogInformation($"Vehicle {vehicle.Id} has landed");
                return;
            }
            else
            {
                _logger.LogInformation($"There are no landing tracks for vehicle {vehicle.Id}");
                Thread.Sleep(5000);
                SimulateLanding(vehicle);
            }
        }

        public async Task<LandingTrack> GetEnabledLandingTrack()
        {
            return await _mediator.Send(new CheckLandingDataCommand());
        }

        #endregion

        #region Initialize database

        private void InitializeDb()
        {
            // Generate fake airport
            GenerateAirport();

            // Generate fake random vehicles
            GenerateVehicles();
        }

        private Airport GenerateAirport()
        {
            Airport airport = new()
            {
                Name = "Miami International airport",
                ControlTowers = new()
                {
                    new()
                    {
                        Name = "Control Tower 1"
                    }
                }
            };

            // Add airport landing tracks
            int landingTracks = 7;
            for (int i = 0; i < landingTracks; i++)
            {
                airport.LandingTracks.Add(new()
                {
                    Name = $"Track #{i + 1}",
                });
            }

            // Save data
            _airportRepository.Add(airport);

            return airport;
        }

        private List<Vehicle> GenerateVehicles()
        {
            var rand = new Random();
            int totalHelicopters = rand.Next(1, 3);
            int totalPlanes = rand.Next(2, 5);

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

        private static void PrintAirportData(Airport airport)
        {
            Dictionary<string, string> printData = new()
            {
                { "Airport", airport.Name },
                { "- Control towers", $"{airport.ControlTowers.Count}" },
                { "- Landing tracks", $"{airport.LandingTracks.Count}" },
            };

            Console.WriteLine();
            foreach (var data in printData)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{data.Key}: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(data.Value);
            }
            Console.WriteLine();
        }

        private static void PrintVehiclesData(List<Vehicle> vehicles)
        {
            Dictionary<string, string> printData;

            Console.WriteLine();
            foreach (var vehicle in vehicles)
            {
                printData = new()
                {
                    { "Vehicle", vehicle.Id },
                    { "- type", $"{vehicle.Type}" },
                    { "- passengers", $"{vehicle.Passengers}" },
                };

                foreach (var data in printData)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{data.Key}: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(data.Value);
                }
            }
            Console.WriteLine();
        }

        #endregion
    }
}
