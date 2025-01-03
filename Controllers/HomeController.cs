using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ab_project.Models;
using ab_project.Services;
using ab_project.Data;

namespace ab_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly ApplicationDbContext _context;

        public HomeController(ITrainingService trainingService, ApplicationDbContext context)
        {
            _trainingService = trainingService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var trainings = await _trainingService.GetAllTrainingsAsync();
            return View(trainings);
        }

        private async Task PopulateCategoriesDropDownList()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
        }

        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesDropDownList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Training training)
        {
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                await _trainingService.AddTrainingAsync(training);
                return RedirectToAction(nameof(Index));
            }
            await PopulateCategoriesDropDownList();
            return View(training);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var training = await _trainingService.GetTrainingByIdAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            await PopulateCategoriesDropDownList();
            return View(training);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Training training)
        {
            if (id != training.TrainingId)
            {
                return NotFound();
            }

            ModelState.Remove("Category");
            if (ModelState.IsValid)
            {
                await _trainingService.UpdateTrainingAsync(training);
                return RedirectToAction(nameof(Index));
            }
            await PopulateCategoriesDropDownList();
            return View(training);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _trainingService.DeleteTrainingAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/instance")]
        public async Task<IActionResult> GetInstanceInfo()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    
                    // Get instance ID
                    var instanceId = await client.GetStringAsync("http://169.254.169.254/latest/meta-data/instance-id");
                    
                    // Get availability zone
                    var availabilityZone = await client.GetStringAsync("http://169.254.169.254/latest/meta-data/placement/availability-zone");
                    
                    // Region is AZ minus the last character
                    var region = availabilityZone[..^1];

                    return Json(new { 
                        InstanceId = instanceId,
                        AvailabilityZone = availabilityZone,
                        Region = region
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { 
                    Error = "Could not retrieve instance metadata",
                    Details = ex.Message 
                });
            }
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
