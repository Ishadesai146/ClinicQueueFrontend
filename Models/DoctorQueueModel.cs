namespace ClinicQueueFrontend.Models
{
    public class DoctorQueueModel
    {
        public int appointmentId { get; set; }
        public int tokenNumber { get; set; }
        public string? status { get; set; }
    }
}
