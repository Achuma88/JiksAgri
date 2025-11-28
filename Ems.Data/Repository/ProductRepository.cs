using JiksAgriFarm.Data.DataAccess;
using JiksAgriFarm.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly ISqlDataAccess _db;

        public ProductRepository(ISqlDataAccess db)
        {
            _db = db;
        }
        public async Task<bool> AddProduct(Product product)
        {
            try
            {
                await _db.SaveData("spAddProduct", new
                {
                    product.ProductName,
                    product.ProductDescription,
                    product.ProductPrice,
                    product.ProductStock,
                    product.ProductUnit,
                    product.ProductStatus
                    
                });

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }
        }
        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                await _db.SaveData("spDeleteProduct", new { ProductID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }
        }
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            try
            {
                var result = await _db.GetData<Product, dynamic>("spGetAllProducts", new { });
                return result ?? new List<Product>();
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return new List<Product>();
            }
        }
        public async Task<Product> GetProductById(int id)
        {
            try
            {
                // Assuming there's a stored procedure like: GetPersonById @EmpID
                var result = await _db.GetData<Product, dynamic>("spGetProductById", new { ProductID = id });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return null;
            }
        }
        public async Task<bool> UpdateProduct(UpdateStock updateStock)
        {
            try
            {
                await _db.SaveData("spUpdateStock", new
                {
                    updateStock.ProductID,
                    updateStock.NewStock
                });

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }
        }
        public async Task<IEnumerable<Product>> GetProductsBySearchTerm(string searchTerm)
        {
            var parameters = new { SearchTerm = searchTerm };
            var result = await _db.GetData<Product, dynamic>("spSearchProducts", parameters);
            return result;
        }
    }
}
