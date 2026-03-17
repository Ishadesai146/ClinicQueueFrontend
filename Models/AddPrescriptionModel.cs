namespace ClinicQueueFrontend.Models
{
    public class AddPrescriptionModel
    {
        public int appointmentId { get; set; }
        public List<PrescriptionMedicine> medicines { get; set; } = new List<PrescriptionMedicine>();
        public string notes { get; set; }
    }

    public class PrescriptionMedicine
    {
        public string name { get; set; }
        public string dosage { get; set; }
        public string duration { get; set; }
    }
}
