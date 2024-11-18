using System.Diagnostics;
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _trainingService.DeleteTrainingAsync(id);
            return RedirectToAction(nameof(Index));
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
