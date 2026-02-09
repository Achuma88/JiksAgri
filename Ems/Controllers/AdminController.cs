using JiksAgriFarm.Data.Models.Domain;
using JiksAgriFarm.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JiksAgriFarm.UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IFarmerRepository _farmerRepostory;
        private readonly IWebHostEnvironment _environment;

        public AdminController(
            IAdminRepository adminRepository,
            IFarmerRepository farmerRepository,
            IWebHostEnvironment environment)
        {
            _adminRepository = adminRepository;
            _farmerRepostory = farmerRepository;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLogin admin)
        {
            if (!ModelState.IsValid)
                return View(admin);

            var admins = await _adminRepository.Login(admin.AdminEmail, admin.AdminPassword);
            if (admins == null)
            {
                ModelState.AddModelError("", "Invalid Email or Password");
                return View(admin);
            }

            HttpContext.Session.SetInt32("AdminID", admins.AdminID);
            HttpContext.Session.SetString("AdminEmail", admin.AdminEmail);

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Admin admin)
        {
            if (!ModelState.IsValid)
                return View(admin);

            admin.AdminStatus = "Active";

            await _adminRepository.Register(admin);
            TempData["Success"] = "Admin successfully registered!";

            return RedirectToAction("Index", "Login");
        }

        public async Task<IActionResult> DisplayAll(string? searchTerm)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminID");
            if (adminId == null)
                return RedirectToAction("Login");

            IEnumerable<Farmer> farmers = string.IsNullOrWhiteSpace(searchTerm)
                ? await _adminRepository.GetAllFarmers()
                : await _adminRepository.SearchFarmerAsync(searchTerm);

            return View(farmers);
        }

        [HttpGet]
        public async Task<IActionResult> ApproveFarmer(int id)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminID");
            if (adminId == null)
                return RedirectToAction("Login");

            var farmer = await _adminRepository.GetFarmerById(id);
            if (farmer == null)
                return NotFound();

            return View(farmer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveFarmerConfirmed(int id)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminID");
            if (adminId == null)
                return RedirectToAction("Login");

            bool success = await _adminRepository.ApproveFarmer(id, adminId.Value);

            if (success)
                TempData["SuccessMessage"] = "Farmer approved successfully.";
            else
                TempData["ErrorMessage"] = "Approval failed. Please try again.";

            return RedirectToAction("DisplayAll");
        }




        /* ============================================================
           VIEW / DOWNLOAD FARMER DOCUMENT
           ============================================================ */
        [HttpGet]
        public async Task<IActionResult> ViewFarmerDocument(int id)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminID");
            if (adminId == null)
                return RedirectToAction("Index", "Login");

            var farmer = await _adminRepository.GetFarmerById(id);

            if (farmer == null || string.IsNullOrEmpty(farmer.DocumentPath))
            {
                TempData["ErrorMessage"] = "No document uploaded for this farmer.";
                return RedirectToAction("ApproveFarmer", new { id });
            }

            // 🔥 FIX: convert web path → physical path
            var relativePath = farmer.DocumentPath.TrimStart('/', '\\');

            var fullPath = Path.Combine(
                _environment.WebRootPath,
                relativePath
            );

            // 🔍 DEBUG CHECK (temporary – remove later)
            Console.WriteLine($"PHYSICAL PATH: {fullPath}");

            if (!System.IO.File.Exists(fullPath))
            {
                TempData["ErrorMessage"] = "Document file not found.";
                return RedirectToAction("ApproveFarmer", new { id });
            }

            var contentType = GetContentType(fullPath);

            return PhysicalFile(
                fullPath,
                contentType,
                Path.GetFileName(fullPath)
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectFarmer(int id, string rejectionReason)
        {
            int? adminId = HttpContext.Session.GetInt32("AdminID");
            if (adminId == null)
                return RedirectToAction("Login");

            if (string.IsNullOrWhiteSpace(rejectionReason))
            {
                TempData["ErrorMessage"] = "Rejection reason is required.";
                return RedirectToAction("ApproveFarmer", new { id });
            }

            await _adminRepository.RejectFarmer(id, rejectionReason, adminId.Value);

            TempData["SuccessMessage"] = "Farmer rejected successfully.";
            return RedirectToAction("DisplayAll");
        }


        private string GetContentType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();

            return ext switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Dashboard()
        {
            var model = await _adminRepository.GetAdminStats();
            return View(model);
        }
    }
}
