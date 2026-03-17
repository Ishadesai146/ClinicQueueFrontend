using ClinicQueueFrontend.Models;
using Newtonsoft.Json;
using System.Text;

namespace ClinicQueueFrontend.Services
{
    public class ApiService
    {
        private HttpClient client;

        public ApiService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://cmsback.sampaarsh.cloud/");
        }

        public async Task<LoginResponse> Login(LoginModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("auth/login", content);

            var data = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<LoginResponse>(data);

            return result;
        }

    }
}
