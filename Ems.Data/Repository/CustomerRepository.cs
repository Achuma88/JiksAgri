using JiksAgriFarm.Data.DataAccess;
using JiksAgriFarm.Data.Models.Domain;
using Microsoft.AspNetCore.Identity;
using JiksAgriFarm.Data.Helpers;
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
        public async Task<bool> Register(Customer customer)
        {
            try
            {
                                customer.DateJoined = DateTime.Now;
                var passwordHasher = new PasswordHasher<Customer>();
                var passwordHash = passwordHasher.HashPassword(customer, customer.CustomerPassword);

                await _db.SaveData("spRegisterCustomer", new
                {
                    customer.CustomerName,
                    customer.CustomerSurname,
                    customer.CustomerPhone,
                    customer.CustomerEmail,
                    customer.CustomerAddress,
                    CustomerPassword = passwordHash   // MUST match SP
                });

                return true;
            }
            catch (Exception ex)
            {
                // log ex
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
        public async Task<Customer?> Login(string email, string password)
        {
            // 1️⃣ Get customer by email
            var customers = await _db.GetData<Customer, dynamic>(
                "spCustomerLogin",
                new { CustomerEmail = email}
            );

            var customer = customers.FirstOrDefault();
            if (customer == null)
                return null;

            // 2️⃣ Verify password
            var passwordHasher = new PasswordHasher<Customer>();
            var result = passwordHasher.VerifyHashedPassword(
                customer,
                customer.CustomerPassword, // hashed password from DB
                password                     // plain password from user
            );

            // 3️⃣ Check result
            return result == PasswordVerificationResult.Success
                ? customer
                : null;
        }



    }
}
