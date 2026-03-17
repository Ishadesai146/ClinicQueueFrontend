namespace ClinicQueueFrontend.Models
{
    public class AddReportModel
    {
        public int appointmentId { get; set; }
        public string diagnosis { get; set; }
        public string testRecommended { get; set; }
        public string remarks { get; set; }
    }
}
