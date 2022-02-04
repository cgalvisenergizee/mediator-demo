using Microsoft.Extensions.Logging;
using Program.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Program.Controllers
{
    public interface IAirportService
    {
        void SimulateLandings();
    }

    public class AirportService : IAirportService
    {
        private readonly ILogger<AirportService> _logger;

        public AirportService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AirportService>();
        }

        public void SimulateLandings()
        {
            _logger.LogInformation($"Simulate landings ...");

            // Generate fake airport
            Airport airport = GenerateAirport();

            // Print airport data
            PrintAirportData(airport);

            // Generate fake random vehicles
            List<Vehicle> vehicles = GenerateVehicles();

            // Print vehicles data
            PrintVehiclesData(vehicles);
        }

        #region General functions

        private static Airport GenerateAirport()
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

            return airport;
        }

        private static List<Vehicle> GenerateVehicles()
        {
            var rand = new Random();
            int totalHelicopters = rand.Next(1, 3);
            int totalPlanes = rand.Next(2, 5);

            List<Vehicle> vehicles = new();

            for (int i = 0; i < totalHelicopters; i++)
                vehicles.Add(new Helicopter()
                {
                    Id = $"H{i + 1}",
                    Passengers = rand.Next(2, 6)
                });

            for (int i = 0; i < totalPlanes; i++)
                vehicles.Add(new Plane()
                {
                    Id = $"P{i + 1}",
                    Passengers = rand.Next(18, 80)
                });

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
                    { "- type", vehicle.GetType().Name },
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
