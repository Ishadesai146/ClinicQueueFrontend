namespace ClinicQueueFrontend.Models
{
    public class PrescriptionModel
    {
        public int id { get; set; }
        public string notes { get; set; }
        public DoctorDetails doctor { get; set; }
        public int appointmentId { get; set; }
        public List<PrescriptionMedicine> medicines { get; set; }
    }

    public class DoctorDetails
    {
        public string name { get; set; }
        public string phone { get; set; }
    }
}