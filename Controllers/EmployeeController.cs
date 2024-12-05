using Employee_inf.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;
using System.Text;

namespace Employee_inf.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public EmployeeController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["apiUrl"];
            _httpClient.BaseAddress = new Uri(configuration["apiUrl"]);
        }

        public IActionResult Create()
        {
            var cultureInfo = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            TempData["DOB"] = DateTime.Today.ToString("yyyy-MM-dd");

            return View();
        }

        public async Task<IActionResult> Save(EmployeeModel employee)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Employee/Save/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Employee saved successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while saving the employee.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(EmployeeModel employee)
        {
            try
            {
                TempData["DOB"] = employee.DOB;
                TempData["Gender"] = employee.Gender;
                TempData["Country"] = employee.Country;
            }
            catch (Exception ex)
            {
                throw;
            }
            return PartialView("_Edit", employee);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EmployeeModel employee)
        {
            employee.Action = "UPDATE";
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Employee/Update/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Employee updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while saving the employee.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete([FromForm] string Id)
        {
            try
            {
                var content = new StringContent($"\"{Id}\"", Encoding.UTF8, "application/json"); // Send as a JSON string
                var response = await _httpClient.PostAsync("/api/Employee/Delete/", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Deleted Successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while saving the employee.";
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Close()
        {
            return RedirectToAction("Index", "Home");
        }


    }
}
