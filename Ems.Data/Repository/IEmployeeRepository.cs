using Ems.Data.Models.Domain;


namespace Ems.Data.Repository
{
    public interface IEmployeeRepository
    {
        Task<bool> AddAsync(Employee employee);
        Task<bool> UpdateEmployeeAsync(EditEmployee editEmployee);
        Task<bool> DeleteAsync(int id);

        Task<Employee> GetByIdAsync(int id);

        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetEmployeeByEmailAsync(string email);
        Task<bool> UpdateInfoAsync(Staff staff);
        Task<bool> UpdateAdminAsync(Admin admin);
        Task<bool> UpdatePasswordAsync(ResetPassword reset);
        Task<Employee> GetProfileAsync(int id);
        Task<Employee> GetAdminProfileAsync(int id);
        //Task<EmployeeSummary> GetSummaryAsync();
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
        Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm);
      
    }
}

