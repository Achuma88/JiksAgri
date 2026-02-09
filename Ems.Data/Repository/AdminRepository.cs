using Dapper;
using JiksAgriFarm.Data.DataAccess;
using JiksAgriFarm.Data.Models.Domain;
using JiksAgriFarm.Data.Repository;

public class AdminRepository : IAdminRepository
{
    private readonly ISqlDataAccess _db;

    public AdminRepository(ISqlDataAccess db)
    {
        _db = db;
    }
    public async Task<bool> Register(Admin admin)
    {
        try
        {
            await _db.SaveData("spRegisterAdmin", new
            {
                admin.AdminName,
                admin.AdminSurname,
                admin.AdminEmail,
                admin.AdminPhone,
                admin.AdminPassword,
                admin.AdminStatus

            });

            return true;
        }
        catch (Exception ex)
        {
            // Log the exception here if needed
            return false;
        }

    }
    public async Task<Admin?> Login(string adminEmail, string adminPassword)
    {
        var result= await _db.GetData<Admin, dynamic>(
            "spAdminLogin",
            new { AdminEmail = adminEmail, AdminPassword = adminPassword }
        );
        return result.FirstOrDefault();
    }

    public async Task<Admin> GetById(int adminId)
    {
        var result = await _db.GetData<Admin, dynamic>(
            "spAdminGetById",
            new { AdminID = adminId }
        );
        return result.FirstOrDefault();
    }
    public async Task<Farmer> GetFarmerById(int farmerId)
    {
        var result = await _db.GetData<Farmer, dynamic>(
            "spGetFarmerById",
            new { FarmerID = farmerId }
        );
        return result.FirstOrDefault();
    }
    public async Task<IEnumerable<Farmer>> GetAllFarmers()
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
    public async Task<IEnumerable<Farmer>> SearchFarmerAsync(string searchTerm)
    {
        var parameters = new { SearchTerm = searchTerm };
        var result = await _db.GetData<Farmer, dynamic>("sp_SearchFarmer", parameters);
        return result;
    }
    public async Task<bool> ApproveFarmer(int farmerId,int adminId)
    {
        try
        {
            await _db.SaveData("spApproveFarmer", new
            {
              FarmerID=farmerId,
              AdminID=adminId

            });

            return true;
        }
        catch (Exception ex)
        {
            // Log the exception here if needed
            return false;
        }

    }
    public async Task<AdminStats> GetAdminStats()
    {
        // Get stats (single row)
        var statsResult = await _db.GetData<AdminStats, dynamic>(
            "spGetAdminStats",
            new { }
        );

        var stats = statsResult.FirstOrDefault() ?? new AdminStats();

        // Get recent farmers
        var recentFarmers = await _db.GetData<RecentFarmer, dynamic>(
            "spGetRecentFarmers",
            new { }
        );

        stats.RecentFarmers = recentFarmers?.ToList() ?? new List<RecentFarmer>();

        return stats;
    }

    public async Task<List<RecentFarmer>> GetRecentFarmers()
    {
        var farmers = await _db.GetData<RecentFarmer, dynamic>(
            "spGetRecentFarmers",
            new { }
        );

        return farmers?.ToList() ?? new List<RecentFarmer>();
    }
    public async Task RejectFarmer(int farmerId, string rejectionReason, int adminId)
    {
        try
        {
            await _db.SaveData("spRejectFarmer", new
            {
                FarmerID = farmerId,
                RejectionReason = rejectionReason,
                AdminID = adminId
            });
        }
        catch (Exception ex)
        {
            // Log exception if needed
            throw;
        }
    }
   










}
