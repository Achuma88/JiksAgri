using JiksAgriFarm.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Repository
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> GetById(int id);
        Task<bool> Register(Customer customer);
        Task<bool> Update(Customer customer);
        Task<bool> Delete(int id);
        Task<Customer> Login(string customerEmail, string customerPassword);

    }
}
