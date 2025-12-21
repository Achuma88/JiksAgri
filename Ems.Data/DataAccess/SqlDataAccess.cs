using Dapper;
using JiksAgriFarm.Data.DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace JiksAgriFarm.Data.DataAccess
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        //This method retrieves All records from the db as well as a singular record.
        public async Task<IEnumerable<T>> GetData<T, P>(string spName, P parameters, string connectionId = "conn")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        //This method works all the inserts, updates and deletes as we are not retrieving data from the db but rather executing the stored procedures 
        public async Task SaveData<T>(string spName, T parameters, string connectionId = "conn")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<T> LoadDataSingle<T>(string storedProcedure, object parameters, string connectionId = "conn")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryFirstOrDefaultAsync<T>(
           storedProcedure,
           parameters,
           commandType: CommandType.StoredProcedure
       );
        }




    }
 }

 

