using JiksAgriFarm.Data.Models.Domain;
using JiksAgriFarm.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JiksAgriFarm.UI.Controllers
{
    public class FarmerController : Controller
    {
        private readonly IFarmerRepository _farmerRepository;

        public FarmerController(IFarmerRepository farmerRepository)
        {
            _farmerRepository = farmerRepository;
        }
        
        public async Task<IActionResult> Add(Farmer farmer)
        {
            if (!ModelState.IsValid)
                return View(farmer);

            bool added = await _farmerRepository.AddAsync(farmer);

            TempData[added ? "SuccessMessage" : "ErrorMessage"] =
                added ? "Product Successfully Added" : "Could not add product";

            return RedirectToAction(nameof(DisplayAll));
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
