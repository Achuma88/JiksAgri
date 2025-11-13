using Dapper;
using Ems.Data.DataAccess;
using Ems.Data.Models.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;

namespace Ems.Data.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ISqlDataAccess _db;

        public LoginRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@EmpEmail", email);
            parameters.Add("@EmpPassword", password);

            // Execute stored procedure that returns 1 if valid, 0 if not
            var result = await _db.GetData<int, DynamicParameters>("sp_ValidateUser", parameters);

            return result.FirstOrDefault() == 1;
        }

    }
}

