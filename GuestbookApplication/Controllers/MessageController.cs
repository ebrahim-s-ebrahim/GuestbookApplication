using Dapper;
using GuestbookApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace GuestbookApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IConfiguration _config;

        public MessageController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Messages>>> GetAllMessages()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            var messages = await connection.QueryAsync<Messages>("select * from messages");
            return Ok(messages);
        }
    }
}
