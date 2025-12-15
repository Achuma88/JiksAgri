using JiksAgriFarm.Data.DataAccess;
using JiksAgriFarm.Data.Models.Domain;
using System;
using System.Collections.Generic;
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

        // Add new Farmer
        public async Task<bool> RegisterAsync(Farmer farmer)
        {
            farmer.RegisteredDate = DateTime.UtcNow;
            try
            {
                await _db.SaveData("spAddFarmer", new
                {
                    farmer.FarmerName,
                    farmer.FarmerPhone,
                    farmer.FarmerEmail,
                    farmer.FarmerPassword,   // hashed before saving
                    farmer.FarmerLocation,
                    farmer.RegisteredDate,
                    farmer.FarmerStatus,
                    IsVerified=false
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Delete Farmer
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _db.SaveData("spDeleteFarmer", new { FarmerID = id });
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Get All Farmers
        public async Task<IEnumerable<Farmer>> GetAllAsync()
        {
            try
            {
                var result = await _db.GetData<Farmer, dynamic>("spGetAllFarmers", new { });
                return result ?? new List<Farmer>();
            }
            catch
            {
                return new List<Farmer>();
            }
        }

        // Get Farmer by ID
        public async Task<Farmer> GetByIdAsync(int id)
        {
            var result = await _db.GetData<Farmer, dynamic>(
                "spGetFarmerById",
                new { FarmerID = id });

            return result.FirstOrDefault();
        }

        // Get Farmer by Email (used in login)
        public async Task<Farmer> GetFarmerByEmailAsync(string email)
        {
            var result = await _db.GetData<Farmer, dynamic>(
                "spGetFarmerByEmail",
                new { FarmerEmail = email });

            return result.FirstOrDefault();
        }

        // Get Farmer Profile
        public async Task<Farmer> GetProfileAsync(int id)
        {
            var result = await _db.GetData<Farmer, dynamic>(
                "spGetFarmerProfile",
                new { FarmerID = id });

            return result.FirstOrDefault();
        }
        public async Task<Farmer> Login(string farmerEmail, string farmerPassword)
        {
            var result = await _db.GetData<Farmer, dynamic>(
                "spFarmerLogin",
                new { FarmerEmail = farmerEmail, FarmerPassword = farmerPassword }
            );
            return result.FirstOrDefault();
        }
    }
}
