using ClinicQueueFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClinicQueueFrontend.Controllers
{
    public class PrescriptionController : Controller
    {
        HttpClient client;

        public PrescriptionController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://cmsback.sampaarsh.cloud/");
        }

        public async Task<IActionResult> My()
        {
            var token = HttpContext.Session.GetString("token");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("prescriptions/my");

            List<PrescriptionModel> list = new List<PrescriptionModel>();

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<PrescriptionModel>>(data);
            }

            return View(list);
        }
    }
}