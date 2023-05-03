using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace PersonInfo.Models
{
    public interface IUserRepository
    {
        Task<int> CreateAsync(User user);
        Task<User> GetAsync(int id);
        Task<List<User>> GetAllUsersAsync();
        Task UpdateAsync(User user);
    }
    public class UserRepository : IUserRepository
    {
        string connectionString = "";
        public UserRepository(string conn)
        {
            connectionString = conn;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var users = await db.QueryAsync<User>("SELECT * FROM Users");
                return users.ToList();
            }
        }

        public async Task<User> GetAsync(int id)
        {
           using (IDbConnection db = new SqlConnection(connectionString))
           {
                var user = await db.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE id = @id", new { id = id });
                return user;
           }
        }

        public async Task<int> CreateAsync(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO Users (FirstName, LastName, DateOfBirth) VALUES (@firstName, @lastName, @dateOfBirth); SELECT CAST(SCOPE_IDENTITY() as int)";

                int? userId =  await db.QueryFirstOrDefaultAsync<int>(query, user);
                user.Id = userId.Value;

                return user.Id;
            }
        }

        public async Task UpdateAsync(User user)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth WHERE Id = @id";
                
                await db.ExecuteAsync(query, user);
            }
        }
    }
}
