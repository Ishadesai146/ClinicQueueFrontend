using Microsoft.AspNetCore.Mvc;
using ClinicQueueFrontend.Models;
using ClinicQueueFrontend.Services;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ClinicQueueFrontend.Controllers
{
    public class AuthController : Controller
    {
        ApiService api = new ApiService();

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await api.Login(model);

            if (result != null)
            {
                HttpContext.Session.SetString("token", result.token);
                HttpContext.Session.SetString("role", result.user.role);

                return RedirectToAction("Dashboard");
            }

            ViewBag.msg = "Login Failed";
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            var token = HttpContext.Session.GetString("token");
            var role = HttpContext.Session.GetString("role");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login");
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://cmsback.sampaarsh.cloud/");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            DashboardViewModel model = new DashboardViewModel();

            if (role == "admin")
            {
                var res = await client.GetAsync("admin/clinic");

                if (res.IsSuccessStatusCode)
                {
                    var data = await res.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<ClinicModel>(data);

                    if (obj != null)
                    {
                        model.TotalUsers = obj.userCount;
                        model.TotalAppointments = obj.appointmentCount;
                        model.TotalQueue = obj.queueCount;
                    }
                }
            }

            else if (role == "patient")
            {
                var res1 = await client.GetAsync("appointments/my");
                if (res1.IsSuccessStatusCode)
                {
                    var data1 = await res1.Content.ReadAsStringAsync();
                    var appointments = JsonConvert.DeserializeObject<List<Appointment>>(data1);
                    model.TotalAppointments = appointments?.Count ?? 0;
                }

                var res2 = await client.GetAsync("prescriptions/my");
                if (res2.IsSuccessStatusCode)
                {
                    var data2 = await res2.Content.ReadAsStringAsync();
                    var prescriptions = JsonConvert.DeserializeObject<List<object>>(data2);
                    model.TotalPrescriptions = prescriptions?.Count ?? 0;
                }

                var res3 = await client.GetAsync("reports/my");
                if (res3.IsSuccessStatusCode)
                {
                    var data3 = await res3.Content.ReadAsStringAsync();
                    var reports = JsonConvert.DeserializeObject<List<object>>(data3);
                    model.TotalReports = reports?.Count ?? 0;
                }
            }

            else if (role == "doctor")
            {
                var res1 = await client.GetAsync("doctor/queue");
                if (res1.IsSuccessStatusCode)
                {
                    var data1 = await res1.Content.ReadAsStringAsync();
                    var q = JsonConvert.DeserializeObject<List<DoctorQueueModel>>(data1);
                    model.TotalAppointments = q?.Count ?? 0;
                }
            }
            
            else if (role == "receptionist")
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                var res1 = await client.GetAsync($"queue?date={today}");
                if (res1.IsSuccessStatusCode)
                {
                    var data1 = await res1.Content.ReadAsStringAsync();
                    var q = JsonConvert.DeserializeObject<List<QueueModel>>(data1);
                    model.TotalQueue = q?.Count ?? 0;
                    model.TotalAppointments = q?.Count ?? 0;
                }
            }

            return View(model);
        }
    }
}