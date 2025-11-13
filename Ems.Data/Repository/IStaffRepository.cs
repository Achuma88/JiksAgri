using Ems.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Data.Repository
{
    public interface IStaffRepository
    {
        Task<Staff> GetEmployeeByEmailAsync(string email);
        Task<Employee> GetByIdAsync(int id);
        Task<bool> UpdateInfoAsync(Staff staff);
        Task<bool> UpdatePasswordAsync(Employee employee);
        Task<Employee> GetProfileAsync(int id);
    }
}
