using JiksAgriFarm.Data.Models.Domain;
using JiksAgriFarm.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace JiksAgriFarm.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // ============================
        // ADD PRODUCT
        // ============================
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Description = new List<string> { "Feed", "Crops", "Dairy", "Butchery", "Supplies" };
            ViewBag.Statuses = new List<string> { "Available", "Out of stock" };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            bool added = await _productRepository.AddProduct(product);

            TempData[added ? "SuccessMessage" : "ErrorMessage"] =
                added ? "Product Successfully Added" : "Could not add product";

            return RedirectToAction(nameof(DisplayAll));
        }

        // ============================
        // DISPLAY ALL
        // ============================
        public async Task<IActionResult> DisplayAll(string? searchTerm)
        {
            IEnumerable<Product> products;

            if (string.IsNullOrWhiteSpace(searchTerm))
                products = await _productRepository.GetAllProducts();
            else
                products = await _productRepository.GetProductsBySearchTerm(searchTerm);

            return View(products);
        }

        // ============================
        // DELETE PRODUCT
        // ============================
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _productRepository.DeleteProduct(id);

            TempData[deleted ? "SuccessMessage" : "ErrorMessage"] =
                deleted ? "Product deleted successfully." : "Error deleting product.";

            return RedirectToAction(nameof(DisplayAll));
        }

        // ============================
        // EDIT STOCK (GET)
        // ============================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
                return NotFound();

            var updateModel = new UpdateStock
            {
                ProductID = product.ProductID,
                NewStock = 0
            };

            return View(updateModel);
        }

        // ============================
        // EDIT STOCK (POST)
        // ============================
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateStock updateStock)
        {
            if (!ModelState.IsValid)
                return View(updateStock);

            bool updated = await _productRepository.UpdateProduct(updateStock);

            TempData[updated ? "SuccessMessage" : "ErrorMessage"] =
                updated ? "Stock updated successfully." : "Stock update failed.";

            return RedirectToAction(nameof(DisplayAll));
        }

        // ============================
        // SEARCH PRODUCTS (AJAX OR FORM)
        // ============================
        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var results = await _productRepository.GetProductsBySearchTerm(searchTerm);
            return View("DisplayAll", results);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
