namespace Program.Models
{
    public class Vehicle
    {
        public string Id { get; set; }
        public int Passengers { get; set; }
        public VehicleType Type { get; set; }
    }

    public enum VehicleType
    {
        Plane,
        Helicopter
    }
}
