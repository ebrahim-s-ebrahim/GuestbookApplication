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

        [HttpGet("messageId")]
        public async Task<ActionResult<Messages>> GetMessage(int messageId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            var message = await connection.QueryFirstAsync<Messages>("select * from messages where messageId = @Id", new { Id = messageId });
            return Ok(message);
        }

        [HttpPost]
        public async Task<ActionResult<List<Messages>>> CreateMessage(Messages messages)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            await connection.ExecuteAsync("insert into messages (messageContent) values (@MessageContent)", messages);
            return Ok(await SelectAllMessages(connection));
        }

        [HttpPut]
        public async Task<ActionResult<List<Messages>>> UpdateMessage(Messages messages)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            await connection.ExecuteAsync("update messages set messageContent = @MessageContent where messageId = @Id", messages);
            return Ok(await SelectAllMessages(connection));
        }

        [HttpDelete("{messageId}")]
        public async Task<ActionResult<List<Messages>>> DeleteMessage(int messageId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("Default"));
            await connection.ExecuteAsync("delete from messages where messageId = @Id", new { Id = messageId });
            return Ok(await SelectAllMessages(connection));
        }





        private static async Task<IEnumerable<Users>> SelectAllMessages(SqlConnection connection)
        {
            return await connection.QueryAsync<Users>("select * from messages");
        }
    }
}
