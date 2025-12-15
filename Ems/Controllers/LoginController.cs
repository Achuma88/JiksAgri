using JiksAgriFarm.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JiksAgriFarm.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAdminRepository _adminRepo;
        private readonly IFarmerRepository _farmerRepo;
        private readonly ICustomerRepository _customerRepo;

        public LoginController(IAdminRepository adminRepo, IFarmerRepository farmerRepo, ICustomerRepository customerRepo)
        {
            _adminRepo = adminRepo;
            _farmerRepo = farmerRepo;
            _customerRepo = customerRepo;
        }

        // LOGIN PAGE
        public IActionResult Index()
        {
            return View();
        }

        // LOGIN POST
        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            // 1️⃣ Admin Login
            var admin = await _adminRepo.Login(email, password);
            if (admin != null)
            {
                HttpContext.Session.SetInt32("AdminID", admin.AdminID);
                HttpContext.Session.SetString("Role", "Admin");

                return RedirectToAction("Dashboard", "Admin");
            }

            // 2️⃣ Farmer Login
            var farmer = await _farmerRepo.Login(email, password);
            if (farmer != null)
            {
                HttpContext.Session.SetInt32("FarmerID", farmer.FarmerID);
                HttpContext.Session.SetString("Role", "Farmer");

                if (farmer.FarmerStatus == "Pending")
                    return RedirectToAction("PendingVerification", "Farmer");

                return RedirectToAction("FarmerHome", "Farmer");
            }

            // 3️⃣ Customer Login
            var customer = await _customerRepo.Login(email, password);
            if (customer != null)
            {
                HttpContext.Session.SetInt32("CustomerID", customer.CustomerID);
                HttpContext.Session.SetString("Role", "Customer");

                return RedirectToAction("CustomerHome", "Customer");
            }

            // ❌ Login failed
            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        
    }
}
