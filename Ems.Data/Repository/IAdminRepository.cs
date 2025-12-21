using JiksAgriFarm.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Repository
{
    public interface IAdminRepository
    {
        Task<Admin?> Login(string adminEmail, string adminPassword);
        Task<bool> Register(Admin admin);
        Task<Admin> GetById(int adminId);
        Task<Farmer> GetFarmerById(int farmerId);
        Task<IEnumerable<Farmer>> GetAllFarmers();
        Task<IEnumerable<Farmer>> SearchFarmerAsync(string searchTerm);
        Task<bool> ApproveFarmer(int farmerId, int adminId);
        Task<dynamic> GetAdminStats();
    }
}
