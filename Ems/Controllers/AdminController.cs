using JiksAgriFarm.Data.Models.Domain;
using JiksAgriFarm.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JiksAgriFarm.UI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IFarmerRepository _farmerRepostory;

        public AdminController(IAdminRepository adminRepository,IFarmerRepository farmerRepository)
        {
            _adminRepository = adminRepository;
            _farmerRepostory = farmerRepository;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Login(AdminLogin admin)
        {
            if(!ModelState.IsValid)
            {
                return View(admin);
            }
            var admins = await _adminRepository.Login(admin.AdminEmail, admin.AdminPassword);
            if (admins==null)
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

            admin.AdminStatus = "Active"; // default status

            var result = await _adminRepository.Register(admin);

            TempData["Success"] = "Admin successfully registered!";
            return RedirectToAction("Index", "Login");
        }
        public async Task<IActionResult> DisplayAll(string? searchTerm)
        {
            IEnumerable<Farmer> farmers;
            int? adminId = HttpContext.Session.GetInt32("AdminID");
            if (adminId == null)
                return RedirectToAction("Login");
            
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                farmers = await _adminRepository.GetAllFarmers();
            }
            else
            {
                // Search by name, email, role, status
                farmers = await _adminRepository.SearchFarmerAsync(searchTerm);
            }

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
            {
                TempData["SuccessMessage"] = "Farmer approved successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to approve farmer. Please try again.";
            }

            return RedirectToAction("DisplayAll");
        }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var stats = await _adminRepository.GetAdminStats();

                if (stats == null)
                {
                    TempData["ErrorMessage"] = "Unable to load admin statistics.";
                    return View(new Admin());
                }

                return View(stats);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while loading admin statistics.";
                return View(new Admin());
            }
        }
    }
}
