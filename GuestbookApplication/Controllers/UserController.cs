using Dapper;
using GuestbookApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace GuestbookApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetAllUsers()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            var users = await connection.QueryAsync<Users>("select * from users");
            return Ok(users);
        }

        [HttpGet("userId")]
        public async Task<ActionResult<Users>> GetUser(int userId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            var user = await connection.QueryFirstAsync<Users>("select * from users where userId = @Id", new { Id = userId});
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<List<Users>>> CreateUser(Users users)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            await connection.ExecuteAsync("insert into users (username, password) values (@Username, @Password)", users);
            return Ok(await SelectAllUsers(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<Users>>> UpdateUser(Users users)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            await connection.ExecuteAsync("update users set username = @UserName, password = @Password where userId = @Id", users);
            return Ok(await SelectAllUsers(connection));
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<List<Users>>> DeleteUser(int userId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            await connection.ExecuteAsync("delete from users where userId = @Id", new { Id = userId });
            return Ok(await SelectAllUsers(connection));
        }





        private static async Task<IEnumerable<Users>> SelectAllUsers(SqlConnection connection)
        {
            return await connection.QueryAsync<Users>("select * from users");
        }
    }
}
