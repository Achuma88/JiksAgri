using JiksAgriFarm.Data.Models.Domain;
using JiksAgriFarm.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JiksAgriFarm.UI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            

            bool added = await _customerRepository.Register(customer);

            TempData[added ? "SuccessMessage" : "ErrorMessage"] =
                added ? "Customer registered successfully" : "Could not register customer";

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> DisplayAll()
        {
            IEnumerable<Customer> customer;

            customer = await _customerRepository.GetAll();

            return View(customer);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var customer = await _customerRepository.Login(login.CustomerEmail, login.CustomerPassword);
            if (customer == null)
            {
                ModelState.AddModelError("", "Invalid Email or Password");
                return View(login);
            }
            HttpContext.Session.SetInt32("CustomerID", customer.CustomerID);
            HttpContext.Session.SetString("CustomerEmail", customer.CustomerEmail);

            return RedirectToAction("Index");
        }
    }
}
