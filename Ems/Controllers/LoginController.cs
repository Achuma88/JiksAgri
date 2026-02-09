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
        [HttpGet]
        public IActionResult Index()
        {
            return View(new Login());
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Login login)
        {
            if (!ModelState.IsValid)
                return View(login);

            string email = login.CustomerEmail?.Trim();
            string password = login.CustomerPassword;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View(login);
            }

            /* ================= ADMIN LOGIN ================= */
            var admin = await _adminRepo.Login(email, password);
            if (admin != null)
            {
                HttpContext.Session.SetInt32("AdminID", admin.AdminID);
                HttpContext.Session.SetString("AdminEmail", admin.AdminEmail);
                HttpContext.Session.SetString("Role", "Admin");

                return RedirectToAction("Index", "Admin");
            }

            /* ================= FARMER LOGIN ================= */
            var farmer = await _farmerRepo.Login(email, password);
            if (farmer != null)
            {
                HttpContext.Session.SetInt32("FarmerID", farmer.FarmerID);
                HttpContext.Session.SetString("Role", "Farmer");

                // ✅ NULL-SAFE SESSION SET
                if (!string.IsNullOrEmpty(farmer.FarmerEmail))
                    HttpContext.Session.SetString("FarmerEmail", farmer.FarmerEmail);

                if (!string.IsNullOrEmpty(farmer.FarmerStatus))
                    HttpContext.Session.SetString("FarmerStatus", farmer.FarmerStatus);

                if (farmer.FarmerStatus == "Pending" || farmer.FarmerStatus == "Rejected")
                    return RedirectToAction("TrackApplication", "Farmer");

                return RedirectToAction("Index", "Farmer");
            }


            /* ================= CUSTOMER LOGIN ================= */
            var customer = await _customerRepo.Login(email, password);
            if (customer != null)
            {
                HttpContext.Session.SetInt32("CustomerID", customer.CustomerID);
                HttpContext.Session.SetString("CustomerName", customer.CustomerName);
                HttpContext.Session.SetString("CustomerSurname", customer.CustomerSurname);
                HttpContext.Session.SetString("CustomerEmail", customer.CustomerEmail);
                HttpContext.Session.SetString("Role", "Customer");

                return RedirectToAction("Index", "Customer");
            }

            /* ================= LOGIN FAILED ================= */
            ModelState.AddModelError("", "Invalid email or password.");
            return View(login);
        }
    }
}
