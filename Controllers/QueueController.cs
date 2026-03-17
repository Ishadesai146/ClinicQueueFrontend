using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using ClinicQueueFrontend.Models;
using System.Text;

namespace ClinicQueueFrontend.Controllers
{
    public class QueueController : Controller
    {
        HttpClient client;

        public QueueController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://cmsback.sampaarsh.cloud/");
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            string today = DateTime.Now.ToString("yyyy-MM-dd");

            var response = await client.GetAsync($"queue?date={today}");

            List<QueueModel> list = new List<QueueModel>();

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<QueueModel>>(data);
            }

            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var token = HttpContext.Session.GetString("token");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var body = new
            {
                status = status
            };

            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PatchAsync($"queue/{id}", content);

            return RedirectToAction("Index");
        }
    }
}