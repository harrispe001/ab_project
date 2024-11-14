using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ab_project.Models;
using ab_project.Services;

namespace ab_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITrainingService _trainingService;

        public HomeController(ITrainingService trainingService)
        {
            _trainingService = trainingService;
        }

        public async Task<IActionResult> Index()
        {
            var trainings = await _trainingService.GetAllTrainingsAsync();
            return View(trainings);
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
