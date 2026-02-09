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
        public async Task<IActionResult> TrackApplication()
        {
            string farmerEmail = HttpContext.Session.GetString("FarmerEmail");

            if (string.IsNullOrEmpty(farmerEmail))
            {
                // Session expired or user not logged in
                return RedirectToAction("Index", "Login");
            }

            var status = await _farmerRepository.GetByEmail(farmerEmail);

            if (status == null)
            {
                ViewBag.Error = "Application not found.";
                return View();
            }

            return View(status);
        }

        [HttpGet]
        public async Task<IActionResult> Reapply()
        {
            int? farmerId = HttpContext.Session.GetInt32("FarmerID");

            if (farmerId == null)
                return RedirectToAction("Index", "Login");

            var farmer = await _farmerRepository.GetByIdAsync(farmerId.Value);

            if (farmer == null)
                return RedirectToAction("Index", "Login");

            return View(farmer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reapply(Farmer farmer, IFormFile DocumentUpload)
        {
            int? farmerId = HttpContext.Session.GetInt32("FarmerID");

            if (farmerId == null)
                return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
                return View(farmer);

            // Ensure correct farmer
            farmer.FarmerID = farmerId.Value;

            // Handle document upload (if provided)
            if (DocumentUpload != null && DocumentUpload.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/farmers");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{DocumentUpload.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await DocumentUpload.CopyToAsync(stream);
                }

                farmer.DocumentPath = "/uploads/farmers/" + fileName;
                farmer.DocumentFileName = Path.GetExtension(DocumentUpload.FileName);
            }

            var success = await _farmerRepository.ReapplyAsync(farmer);

            if (!success)
            {
                ModelState.AddModelError("", "Reapplication failed. Please try again.");
                return View(farmer);
            }

            TempData["SuccessMessage"] = "Reapplication submitted successfully. Please wait for admin review.";
            return RedirectToAction("TrackApplication");
        }



    }
}
