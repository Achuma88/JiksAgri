using JiksAgriFarm.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Product>> GetProductsBySearchTerm(string? searchTerm);
        Task<bool> AddProduct(Product product);
        Task<bool> UpdateProduct(UpdateStock updateStock);
        Task<bool> DeleteProduct(int id);
    }
}
