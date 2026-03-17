using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ClinicQueueFrontend.Models;
using System.Net.Http.Headers;
using System.Text;

namespace ClinicQueueFrontend.Controllers
{
    public class AppointmentController : Controller
    {
        HttpClient client;

        public AppointmentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://cmsback.sampaarsh.cloud/");
        }
        public async Task<IActionResult> MyAppointments()
        {
            var token = HttpContext.Session.GetString("token");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("appointments/my");

            List<Appointment> list = new List<Appointment>();

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<Appointment>>(data);
            }

            return View(list);
        }

        public IActionResult Book()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Book(BookAppointmentModel model)
        {
            var token = HttpContext.Session.GetString("token");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("appointments", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MyAppointments");
            }

            var error = await response.Content.ReadAsStringAsync();
            ViewBag.msg = error;

            return View();
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"appointments/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var appointment = JsonConvert.DeserializeObject<Appointment>(data);
                return View(appointment);
            }

            return RedirectToAction("MyAppointments");
        }
    }
}