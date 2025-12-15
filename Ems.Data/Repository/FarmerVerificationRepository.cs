using JiksAgriFarm.Data.DataAccess;
using JiksAgriFarm.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiksAgriFarm.Data.Repository
{
    public class FarmerVerificationRepository : IFarmerVerificationRepository
    {
        private readonly ISqlDataAccess _db;

        public FarmerVerificationRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        // Add verification record
        public async Task<bool> AddVerificationAsync(FarmerVerification verification)
        {
            try
            {
                await _db.SaveData("spAddFarmerVerification", new
                {
                    verification.FarmerID,
                    verification.DocumentPath,
                    verification.UploadedDate,
                    verification.VerificationStatus
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Get verification by FarmerID
        public async Task<FarmerVerification> GetByFarmerIdAsync(int farmerId)
        {
            var result = await _db.GetData<FarmerVerification, dynamic>(
                "spGetVerificationByFarmerId",
                new { FarmerID = farmerId });

            return result.FirstOrDefault();
        }

        // Get all pending verification requests (Admin)
        public async Task<IEnumerable<FarmerVerification>> GetAllPendingAsync()
        {
            var result = await _db.GetData<FarmerVerification, dynamic>(
                "spGetPendingVerifications",
                new { });

            return result ?? new List<FarmerVerification>();
        }

        // Approve verification
        public async Task<bool> ApproveAsync(int verificationId)
        {
            try
            {
                await _db.SaveData("spApproveFarmerVerification", new
                {
                    VerificationID = verificationId
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Reject verification
        public async Task<bool> RejectAsync(int verificationId, string reason)
        {
            try
            {
                await _db.SaveData("spRejectFarmerVerification", new
                {
                    VerificationID = verificationId,
                    RejectionReason = reason
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
