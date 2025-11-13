using Ems.Data.DataAccess;
using Ems.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Data.Repository
{
    public class StaffRepository:IStaffRepository
    {
        private readonly ISqlDataAccess _db;

        public StaffRepository(ISqlDataAccess db)
        {
            _db = db;
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
        public async Task<bool> UpdatePasswordAsync(Employee employee)
        {
            try
            {
                await _db.SaveData("spChangePassword", new
                {
                    employee.EmpID,
                    employee.EmpPassword

                });

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }
        }
        public async Task<Staff> GetEmployeeByEmailAsync(string email)
        {
            var result = await _db.GetData<Staff, dynamic>("GetEmployeeByEmail",
                new { EmpEmail = email });

            return result.FirstOrDefault();
        }
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
        public async Task<Employee> GetProfileAsync(int id)
        {
            var profResult = await _db.GetData<Employee, dynamic>("spGetStaffProfile", new { EmpID = id });
            return profResult.FirstOrDefault();
        }
    }
}
