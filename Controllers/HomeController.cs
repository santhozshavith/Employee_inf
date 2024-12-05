using Employee_inf.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace Employee_inf.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public HomeController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["apiUrl"];
            _httpClient.BaseAddress = new Uri(configuration["apiUrl"]);

        }

        public async Task<IActionResult> Index(EmployeeModel employee)
        {
            List<EmployeeModel> employees = new List<EmployeeModel>();
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/Employee/GetEmployeeList/", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var employeeResponse = JsonConvert.DeserializeObject<EmployeeResponse>(jsonResponse);
                    employees = employeeResponse.EmployeeId;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to load employee list.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, "An error occurred while fetching data.");
            }

            return View("Index", employees); 
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
