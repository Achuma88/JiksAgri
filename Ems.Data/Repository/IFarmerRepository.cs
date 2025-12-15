using JiksAgriFarm.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JiksAgriFarm.Data.Repository
{
    public interface IFarmerRepository
    {
        Task<IEnumerable<Farmer>> GetAllAsync();
        Task<Farmer> GetByIdAsync(int id);
        Task<bool> RegisterAsync(Farmer farmer);
        //Task<bool> UpdateAsync(Farmer farmer);
        Task<bool> DeleteAsync(int id);
        Task<Farmer> GetFarmerByEmailAsync(string email);
        Task<Farmer> Login(string farmerEmail, string farmerPassword);
    }
}
