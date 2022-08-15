using Dapper;
using GuestbookApplication.DTOs;
using GuestbookApplication.Models;
using GuestbookApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace GuestbookApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly SqlConnection _context;

        public MessageController(GuestBookContext connection)
        {
            _context = connection.DbContext;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<MessageViewModel>>> GetAllMessages()
        {
            try
            {
                var messages = await _context.QueryAsync<MessageViewModel>("select * from messages");
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a certain message by its Id
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>the message and the userId who wrote the message</returns>
        [HttpGet("messageId")]
        public async Task<ActionResult<MessageViewModel>> GetMessageById(int messageId)
        {
            try
            {
                if (messageId < 1)
                    return BadRequest("Invalid message id.");

                var message = await _context.QueryFirstAsync<MessageViewModel>("select * from messages where messageId = @Id", new { Id = messageId });
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Write a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>All messsages with the created one</returns>
        [HttpPost("WriteMessage")]
        public async Task<ActionResult<MessageViewModel>> CreateMessage(MessageDTO message)
        {
            try
            {
                if (message == null)
                    return BadRequest("Invalid message.");

                await _context.ExecuteAsync("insert into messages (messageContent, userId) values (@MessageContent, @UserId)", message);
                return Ok(await SelectAllMessages());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Edit a certain message
        /// </summary>
        /// <param name="updatedMessage"></param>
        /// <returns>The edited message with its info</returns>
        [HttpPut("EditMessage")]
        public async Task<ActionResult<MessageViewModel>> UpdateMessage(MessageUpdateDTO updatedMessage)
        {
            try
            {
                await _context.ExecuteAsync("update messages set messageContent = @MessageContent where messageId = @Id", new { MessageContent = updatedMessage.MessageContent, Id = updatedMessage.MessageId });
                var message = await _context.QueryFirstAsync<MessageViewModel>("select * from messages where messageId = @Id", new { Id = updatedMessage.MessageId });
                return Ok(message);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{messageId}")]
        public async Task<ActionResult> DeleteMessage(int messageId)
        {
            try
            {
                await _context.ExecuteAsync("delete from messages where messageId = @Id", new { Id = messageId });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Add a reply to a certain message
        /// </summary>
        /// <param name="reply"></param>
        /// <returns>a list of the message with te replies on it</returns>
        [HttpPost("ReplyToMessage")]
        public async Task<ActionResult<List<ReplyViewModel>>> ReplyToMessage(ReplyDTO reply)
        {
            try
            {
                if (reply == null)
                    return BadRequest("Invalid message.");

                await _context.ExecuteAsync("insert into messages (messageContent, userId, parentMessageId) values (@MessageContent, @UserId,@ParentMessageId)", reply);

                return Ok(await SelectAllMessages());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// get replies on a certain message
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns>replies of the message</returns>
        [HttpGet("GetReplies/{messageId}")]
        public async Task<ActionResult<ReplyViewModel>> GetMessageReplies(int messageId)
        {
            try
            {
                if (messageId < 1)
                    return BadRequest("Invalid message id.");

                var parent = await _context.QueryFirstAsync<ReplyViewModel>("select * from messages where messageId = @Id", new { Id = messageId });
                if (parent is null)
                    return NotFound("Message not found.");

                parent.Children = (await _context.QueryAsync<MessageViewModel>("select * from messages where ParentMessageId = @Id", new { Id = messageId })).ToList();
                return Ok(parent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        private async Task<IEnumerable<MessageViewModel>> SelectAllMessages()
        {
            return await _context.QueryAsync<MessageViewModel>("select * from messages");
        }
    }
}