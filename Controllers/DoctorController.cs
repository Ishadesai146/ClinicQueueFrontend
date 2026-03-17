using ClinicQueueFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ClinicQueueFrontend.Controllers
{
    public class DoctorController : Controller
    {
        HttpClient client;

        public DoctorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://cmsback.sampaarsh.cloud/");
        }

        public async Task<IActionResult> Queue()
        {
            var token = HttpContext.Session.GetString("token");

            

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var res = await client.GetAsync("doctor/queue");

            List<DoctorQueueModel> list = new List<DoctorQueueModel>();

            if (res.IsSuccessStatusCode)
            {
                var data = await res.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<DoctorQueueModel>>(data);
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult AddPrescription(int appointmentId)
        {
            var model = new AddPrescriptionModel { appointmentId = appointmentId };
           
            model.medicines.Add(new PrescriptionMedicine());
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription(AddPrescriptionModel model)
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var body = new
            {
                medicines = model.medicines.Where(m => !string.IsNullOrEmpty(m.name)).ToArray(),
                notes = model.notes
            };

            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"prescriptions/{model.appointmentId}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Queue");
            }
            
            var errorResult = await response.Content.ReadAsStringAsync();
            try
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(errorResult);
                ViewBag.msg = dict.ContainsKey("error") ? dict["error"] : errorResult;
            }
            catch
            {
                ViewBag.msg = errorResult;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddReport(int appointmentId)
        {
            var model = new AddReportModel { appointmentId = appointmentId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReport(AddReportModel model)
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var body = new
            {
                diagnosis = model.diagnosis,
                testRecommended = model.testRecommended,
                remarks = model.remarks
            };

            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"reports/{model.appointmentId}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Queue");
            }

            var errorResult = await response.Content.ReadAsStringAsync();
            try
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(errorResult);
                ViewBag.msg = dict.ContainsKey("error") ? dict["error"] : errorResult;
            }
            catch
            {
                ViewBag.msg = errorResult;
            }
            return View(model);
        }
    }
}