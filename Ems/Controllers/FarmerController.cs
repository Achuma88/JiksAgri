using JiksAgriFarm.Data.Models.Domain;
using JiksAgriFarm.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace JiksAgriFarm.UI.Controllers
{
    public class FarmerController : Controller
    {
        private readonly IFarmerRepository _farmerRepository;

        public FarmerController(IFarmerRepository farmerRepository)
        {
            _farmerRepository = farmerRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Farmer farmer, IFormFile DocumentUpload)
        {
            if (ModelState.IsValid)
            {
                var farmerId = await _farmerRepository.RegisterAsync(farmer);

                TempData["SuccessMessage"] = "Registration submitted. Please wait for verification.";
                return RedirectToAction(nameof(Index));
            }

            return View(farmer);
        }
        public async Task<IActionResult> DisplayAll(string? searchTerm)
        {
            IEnumerable<Farmer> farmer;

          farmer = await _farmerRepository.GetAllAsync();
           
            return View(farmer);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
