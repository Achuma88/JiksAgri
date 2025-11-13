using Ems.Data.DataAccess;
using Ems.Data.Models.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Ems.Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ISqlDataAccess _db;

        public EmployeeRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        // Add Employee
        public async Task<bool> AddAsync(Employee employee)
        {
            try
            {
                await _db.SaveData("sp_AddEmployee", new
                {
                    employee.EmpName,
                    employee.EmpSurname,
                    employee.EmpEmail,
                    employee.EmpPhone,
                    employee.EmpPassword,
                    employee.EmpRole,
                    employee.EmpStatus
                });

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }
        }

        // Delete Employee by ID
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _db.SaveData("sp_DeleteEmployee", new { EmpID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }
        }

        // Get All Employees
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                var result = await _db.GetData<Employee, dynamic>("sp_GetAllEmployees", new { });
                return result ?? new List<Employee>();
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return new List<Employee>();
            }
        }

        // Get Employee by ID
        public async Task<Employee> GetByIdAsync(int id)
        {
            try
            {
                // Assuming there's a stored procedure like: GetPersonById @EmpID
                var result = await _db.GetData<Employee, dynamic>("sp_GetEmployeeById", new { EmpID = id });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return null;
            }
        }
   

        // Update Employee
        public async Task<bool> UpdateEmployeeAsync(EditEmployee editEmployee)
        {
            try
            {
                await _db.SaveData("sp_UpdateEmployee", new
                {
                    editEmployee.EmpID,
                    editEmployee.EmpRole,
                    editEmployee.EmpStatus
                });

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }
        }
        public async Task<Employee> GetEmployeeByEmailAsync(string email)
        {
            var result = await _db.GetData<Employee, dynamic>("spGetEmployeeByEmail",
                new { EmpEmail = email });

            return result.FirstOrDefault();
        }
        public async Task<Employee> GetProfileAsync(int id)
        {
            var profResult = await _db.GetData<Employee, dynamic>("spGetStaffProfile", new { EmpID = id });
            return profResult.FirstOrDefault();
        }
        public async Task<Employee> GetAdminProfileAsync(int id)
        {
            var adProf = await _db.GetData<Employee, dynamic>("spGetAdminProfile", new { EmpID = id });
            return adProf.FirstOrDefault();
        }
        public async Task<bool> UpdatePasswordAsync(ResetPassword reset)
        {
            try
            {
                await _db.SaveData("spChangePassword", new
                {
                    EmpID = reset.EmpID,
                    EmpPassword = reset.EmpPassword
                });

                return true;
            }
            catch (Exception ex)
            {
                // Optional: log exception
                return false;
            }
        }

        public async Task<bool> UpdateInfoAsync(Staff staff)
        {
            try
            {
                await _db.SaveData("spUpdateStaffInfo", new
                {
                    staff.EmpID,
                    staff.EmpName,
                    staff.EmpSurname,
                    staff.EmpEmail,
                    staff.EmpPhone,

                });

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }
        }
        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        {
            var result = await _db.GetData<Employee, dynamic>("sp_GetActiveEmployees", new { });

            return result;
        }
        public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm)
        {
            var parameters = new { SearchTerm = searchTerm };
            var result = await _db.GetData<Employee, dynamic>("sp_SearchEmployees", parameters);
            return result;
        }
        public async Task<bool> UpdateAdminAsync(Admin admin)
        {
            try
            {
                await _db.SaveData("spUpdateStaffInfo", new
                {
                    admin.EmpID,
                    admin.EmpName,
                    admin.EmpSurname,
                    admin.EmpEmail,
                    admin.EmpPhone,

                });

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }
        }


    }
}
