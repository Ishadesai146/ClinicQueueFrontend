namespace ClinicQueueFrontend.Models
{
    public class ClinicModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string createdAt { get; set; }
        public int userCount { get; set; }
        public int appointmentCount { get; set; }
        public int queueCount { get; set; }
    }
}