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
        public async Task<IActionResult> Register(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            bool added = await _customerRepository.Register(customer);

            TempData[added ? "SuccessMessage" : "ErrorMessage"] =
                added ? "Product Successfully Added" : "Could not add product";

            return RedirectToAction(nameof(DisplayAll));
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
    }
}
