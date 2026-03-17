namespace ClinicQueueFrontend.Models
{
    public class ReportModel
    {
        public int id { get; set; }
        public string diagnosis { get; set; }
        public string testRecommended { get; set; }
        public string remarks { get; set; }
        public DoctorDetails doctor { get; set; }
        public int appointmentId { get; set; }
    }
}
