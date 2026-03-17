namespace ClinicQueueFrontend.Models
{
    public class LoginResponse
    {
        public string token { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public int clinicId { get; set; }
        public string clinicName { get; set; }
        public string clinicCode { get; set; }
    }
}