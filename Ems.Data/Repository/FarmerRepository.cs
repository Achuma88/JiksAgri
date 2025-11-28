using JiksAgriFarm.Data.DataAccess;
using JiksAgriFarm.Data.Models.Domain;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Repository
{
    public class FarmerRepository : IFarmerRepository
    {
        private readonly ISqlDataAccess _db;

        public FarmerRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        // Add Farmer to the system
        public async Task<bool> AddAsync(Farmer farmer)
        {
            try
            {
                await _db.SaveData("spAddFarmer", new
                {
                    farmer.FarmerName,
                    farmer.FarmerPhone,
                    farmer.FarmerEmail,
                    farmer.FarmerLocation,
                    farmer.DateRegistered,
                    farmer.FarmerStatus
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
                await _db.SaveData("spDeleteFarmer", new { FarmerID = id });
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return false;
            }
        }

        // Get All Employees
        public async Task<IEnumerable<Farmer>> GetAllAsync()
        {
            try
            {
                var result = await _db.GetData<Farmer, dynamic>("spGetAllFarmers", new { });
                return result ?? new List<Farmer>();
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return new List<Farmer>();
            }
        }

        // Get Employee by ID
        public async Task<Farmer> GetByIdAsync(int id)
        {
            try
            {
                // Assuming there's a stored procedure like: GetPersonById @EmpID
                var result = await _db.GetData<Farmer, dynamic>("sp_GetEmployeeById", new { FarmerID = id });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return null;
            }
        }


        // Update Employee
        //public async Task<bool> UpdateEmployeeAsync(EditEmployee editEmployee)
        //{
        //    try
        //    {
        //        await _db.SaveData("sp_UpdateEmployee", new
        //        {
        //            editEmployee.EmpID,
        //            editEmployee.EmpRole,
        //            editEmployee.EmpStatus
        //        });

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception here if needed
        //        return false;
        //    }
        //}
        public async Task<Farmer> GetFarmerByEmailAsync(string email)
        {
            var result = await _db.GetData<Farmer, dynamic>("spGetFarmerByEmail",
                new { FarmerEmail = email });

            return result.FirstOrDefault();
        }
        public async Task<Farmer> GetProfileAsync(int id)
        {
            var profResult = await _db.GetData<Farmer, dynamic>("spGetStaffProfile", new {FarmerID = id });
            return profResult.FirstOrDefault();
        }

        //public async Task<bool> UpdatePasswordAsync(ResetPassword reset)
        //{
        //    try
        //    {
        //        await _db.SaveData("spChangePassword", new
        //        {
        //            EmpID = reset.EmpID,
        //            EmpPassword = reset.EmpPassword
        //        });

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Optional: log exception
        //        return false;
        //    }
        //}

        //public async Task<bool> UpdateInfoAsync(Staff staff)
        //{
        //    try
        //    {
        //        await _db.SaveData("spUpdateStaffInfo", new
        //        {
        //            staff.EmpID,
        //            staff.EmpName,
        //            staff.EmpSurname,
        //            staff.EmpEmail,
        //            staff.EmpPhone,

        //        });

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception here if needed
        //        return false;
        //    }
        //}
        //public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        //{
        //    var result = await _db.GetData<Employee, dynamic>("sp_GetActiveEmployees", new { });

        //    return result;
        //}
        //public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm)
        //{
        //    var parameters = new { SearchTerm = searchTerm };
        //    var result = await _db.GetData<Employee, dynamic>("sp_SearchEmployees", parameters);
        //    return result;
        //}



    }
}
