using JiksAgriFarm.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Repository
{
    public interface IFarmerVerificationRepository
    {
        Task<bool> AddVerificationAsync(FarmerVerification verification);
        Task<FarmerVerification> GetByFarmerIdAsync(int farmerId);
        Task<IEnumerable<FarmerVerification>> GetAllPendingAsync();
        Task<bool> ApproveAsync(int verificationId);
        Task<bool> RejectAsync(int verificationId, string reason);
    }
}
