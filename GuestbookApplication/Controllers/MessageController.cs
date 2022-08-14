﻿using Dapper;
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

        [HttpGet]
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

        [HttpPost]
        public async Task<ActionResult<MessageViewModel>> CreateMessage(MessageDTO messages)
        {
            try
            {
                if (messages == null)
                    return BadRequest("Invalid message.");

                await _context.ExecuteAsync("insert into messages (messageContent) values (@MessageContent)", messages);
                return Ok(await SelectAllMessages());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
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

        private async Task<IEnumerable<MessageViewModel>> SelectAllMessages()
        {
            return await _context.QueryAsync<MessageViewModel>("select * from messages");
        }
    }
}