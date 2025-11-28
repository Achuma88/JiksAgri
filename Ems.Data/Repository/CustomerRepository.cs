using JiksAgriFarm.Data.DataAccess;
using JiksAgriFarm.Data.Models.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Repository
{
    public class CustomerRepository:ICustomerRepository
    {
        private readonly ISqlDataAccess _db;

        public CustomerRepository(ISqlDataAccess db)
        {
            _db = db;
        }
        public async Task<bool> Add(Customer customer)
        {
            try
            {
                await _db.SaveData("spAddCustomer", new
                {
                    customer.CustomerName,
                    customer.CustomerPhone,
                    customer.CustomerEmail,
                    customer.CustomerAddress,
                    customer.DateJoined
                });

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }
        }
        
        public async Task<bool> Update(Customer customer)
        {
            try
            {
                await _db.SaveData("spUpdateCustomerInfo", new
                {
                    customer.CustomerID,
                    customer.CustomerName,
                    customer.CustomerPhone,
                    customer.CustomerEmail,
                    customer.CustomerAddress
                });

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                await _db.SaveData("sp_DeleteEmployee", new { CustomerID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }

        }
        public async Task<IEnumerable<Customer>> GetAll()
        {
            try
            {
                var result = await _db.GetData<Customer, dynamic>("spGetAllCustomers", new { });
                return result ?? new List<Customer>();
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return new List<Customer>();
            }

        }
        public async Task<Customer> GetById(int id)
        {
            try
            {
                // Assuming there's a stored procedure like: GetPersonById @EmpID
                var result = await _db.GetData<Customer, dynamic>("spGetCustomerById", new { CustomerID = id });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return null;
            }

        }
    }
}
