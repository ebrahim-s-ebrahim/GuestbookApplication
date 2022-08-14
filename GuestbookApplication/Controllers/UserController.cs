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
    public class UserController : ControllerBase
    {
        private readonly SqlConnection _context;

        public UserController(GuestBookContext connection)
        {
            _context = connection.DbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserViewModel>>> GetAllUsers()
        {
            try
            {
                var users = await _context.QueryAsync<UserViewModel>("select * from users");
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Gets user by his / her Id
        /// </summary>
        /// <param name="userId">The Id to get user with</param>
        /// <returns>The user with sent Id</returns>
        [HttpGet("userId")]
        public async Task<ActionResult<UserViewModel>> GetUserById(int userId)
        {
            try
            {
                if (userId < 1)
                    return BadRequest("Invalid user id.");

                var user = await _context.QueryFirstAsync<UserViewModel>("select * from users where userId = @Id", new { Id = userId });
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserViewModel>> CreateUser(UserDTO user)
        {
            try
            {
                if (user == null)
                    return BadRequest("Invalid user.");

                await _context.ExecuteAsync("insert into users (username, password) values (@Username, @Password)", user);
                return Ok(await SelectAllUsers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserViewModel>> Login(UserDTO user)
        {
            try
            {
                if (user == null)
                    return BadRequest("Invalid user.");

                var existingUser = await _context.QueryAsync<User>("select * from users where userName = @Username and password = @Password", new { UserName = user.UserName, Password = user.Password });

                if (existingUser is null || !existingUser.Any())
                    return Unauthorized("Invalid username or password");

                return Ok("Successful login");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<UserViewModel>> UpdateUser(UserUpdateDTO user)
        {
            try
            {
                if (user == null)
                    return BadRequest("Invalid user.");

                await _context.ExecuteAsync("update users set username = @UserName, password = @Password where userid = @Id", new { UserName = user.UserName, Password = user.Password, Id = user.UserId});
                return Ok(await SelectAllUsers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            try
            {
                if (userId < 1)
                    return BadRequest("Invalid user id.");

                await _context.ExecuteAsync("delete from users where userId = @Id", new { Id = userId });
                return Ok(await SelectAllUsers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllMessagesByUserID")]
        public async Task<ActionResult<List<MessageViewModel>>> GetAllMessagesByUserID(int userId)
        {
            try
            {
                if (userId < 1)
                    return BadRequest("Invalid user id.");

                var message = await _context.QueryFirstAsync<MessageViewModel>("select * from messages where userId = @Id", new { Id = userId });
                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private async Task<IEnumerable<UserViewModel>> SelectAllUsers()
        {
            return await _context.QueryAsync<UserViewModel>("select * from users");
        }
    }
}
