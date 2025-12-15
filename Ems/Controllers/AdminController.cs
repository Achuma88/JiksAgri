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

            return RedirectToAction("DisplayAll");
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
        public async Task<IActionResult> ApproveFarmer(int farmerId)
        {
            int ?adminId = HttpContext.Session.GetInt32("AdminID");
            if (adminId == null)
            {
                return RedirectToAction("Login");
            }
            bool result = await _adminRepository.ApproveFarmer(farmerId, adminId.Value);
            if (result)
            {
                TempData["SuccessApprove"] = "Farmer Approved Successfully";
            }
            else
            {
                TempData["Error"] = "Farmer Not Approved";
            }

            return RedirectToAction("DisplayAll");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
