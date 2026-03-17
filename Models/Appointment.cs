namespace ClinicQueueFrontend.Models
{
    public class Appointment
    {
        public int id { get; set; }
        public string appointmentDate { get; set; }
        public string timeSlot { get; set; }
        public string status { get; set; }
        public int patientId { get; set; }
        public int clinicId { get; set; }
        public QueueEntry queueEntry { get; set; }
        public PrescriptionModel prescription { get; set; }
        public ReportModel report { get; set; }
    }

    public class QueueEntry
    {
        public int id { get; set; }
        public int tokenNumber { get; set; }
        public string status { get; set; }
        public string queueDate { get; set; }
        public int appointmentId { get; set; }
        public AppointmentDetails appointment { get; set; }
    }

    public class AppointmentDetails
    {
        public PatientDetails patient { get; set; }
    }

    public class PatientDetails
    {
        public string name { get; set; }
        public string phone { get; set; }
    }
}
