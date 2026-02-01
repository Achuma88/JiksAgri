using JiksAgriFarm.Data.Repository;
using JiksAgriFarm.Data.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace JiksAgriFarm.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAdminRepository _adminRepo;
        private readonly IFarmerRepository _farmerRepo;
        private readonly ICustomerRepository _customerRepo;

        public LoginController(
            IAdminRepository adminRepo,
            IFarmerRepository farmerRepo,
            ICustomerRepository customerRepo)
        {
            _adminRepo = adminRepo;
            _farmerRepo = farmerRepo;
            _customerRepo = customerRepo;
        }

        // GET
        public IActionResult Index()
        {
            return View(new Login());
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Index(Login login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var email = login.CustomerEmail.Trim();
            var password = login.CustomerPassword;

            // 1️⃣ ADMIN LOGIN
            var admin = await _adminRepo.Login(email, password);
            if (admin != null)
            {
                HttpContext.Session.SetInt32("AdminID", admin.AdminID);
                HttpContext.Session.SetString("Role", "Admin");

                return RedirectToAction("Index", "Admin");
            }

            // 2️⃣ FARMER LOGIN
            var farmer = await _farmerRepo.Login(email, password);
            if (farmer != null)
            {
                HttpContext.Session.SetInt32("FarmerID", farmer.FarmerID);
                HttpContext.Session.SetString("Role", "Farmer");

                if (farmer.FarmerStatus == "Pending")
                    return RedirectToAction("PendingVerification", "Farmer");

                return RedirectToAction("Dashboard", "Farmer");
            }

            // 3️⃣ CUSTOMER LOGIN (YOUR LOGIC INCORPORATED ✅)
            var customer = await _customerRepo.Login(email, password);
            if (customer != null)
            {
                HttpContext.Session.SetInt32("CustomerID", customer.CustomerID);
                HttpContext.Session.SetString("CustomerName", customer.CustomerName);
                HttpContext.Session.SetString("CustomerSurname", customer.CustomerSurname);
                HttpContext.Session.SetString("CustomerEmail", customer.CustomerEmail);
              

                return RedirectToAction("Index", "Customer");
            }

            // ❌ LOGIN FAILED
            ModelState.AddModelError("", "Invalid Email or Password");
            return View(login);
        }
    }
}
