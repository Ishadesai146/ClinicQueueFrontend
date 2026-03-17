using ClinicQueueFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ClinicQueueFrontend.Controllers
{
    public class AdminController : Controller
    {
        HttpClient client;

        public AdminController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://cmsback.sampaarsh.cloud/");
        }
        public async Task<IActionResult> Clinic()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("admin/clinic");

            ClinicModel clinic = null;

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                clinic = JsonConvert.DeserializeObject<ClinicModel>(data);
            }

            return View(clinic);
        }

        public async Task<IActionResult> Users()
        {
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("admin/users");

            List<UserModel> users = new List<UserModel>();

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<UserModel>>(data);
            }

            return View(users);
        }

        // Create User Page

        public IActionResult CreateUser()
        {
            return View();
        }


        // Create User POST

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserModel model)
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (model == null)
            {
                ViewBag.msg = "Invalid data";
                return View();
            }

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(model);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("admin/users", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Users");
            }

            var error = await response.Content.ReadAsStringAsync();

            if (error.Contains("Forbidden"))
            {
                ViewBag.msg = "You are not allowed to create users (Admin permission required)";
            }
            else
            {
                ViewBag.msg = error;
            }

            return View();
        }
    }
}