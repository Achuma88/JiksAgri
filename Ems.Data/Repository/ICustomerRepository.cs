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
        Task<bool> Add(Customer customer);
        Task<bool> Update(Customer customer);
        Task<bool> Delete(int id);
    }
}
