﻿
using BusinessLayer_BankManagementSystem.Services;
using DataAccessLayer_BankManagementSystem.Data;
using DataAccessLayer_BankManagementSystem.DTO;
using DataAccessLayer_BankManagementSystem.Entities;
using DataAccessLayer_BankManagementSystem.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Back_End_Bank_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        readonly IUserRepository<User> _userRepository;

        readonly ApplicationDbContext _context;
        public UserController(IUserRepository<User> userRepository,ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        [HttpGet]
        public async Task <IActionResult> Get()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet("{Id}")]
        public async Task <IActionResult> Get(int Id)
        {
            var user = await _userRepository.GetByUsersinfobyIdAsync(Id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpDelete("{Id}")]
        public async Task  <IActionResult> Delete(int Id)
        {

            var user =await  _context.Users.FindAsync(Id);

            if (user == null)
                return NotFound(); // 404 if product doesn't exist

            // Delete the product
           await _userRepository.DeleteUserAsync(user);

            // Return 200 OK with the deleted product in the response body
            return Ok(user);

        }

        [HttpPut]
        public async Task <IActionResult>Update(User user)
        {


            var updateduser =await _userRepository.UpdateUsersAsync(user);

            return Ok(updateduser);

        }
        [HttpPost]
        public async Task <IActionResult> Add(User user)
        {

            var users = await _userRepository.AddUsersAsync(user);

            return Ok(users);
        }

        [HttpPost("login")]
        public async Task <IActionResult>Login([FromBody] LoginRequest request)
        {

            var user = await _userRepository.FindUsersbyUserNameAndPasswordAsync(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Username or password is incorrect" });
            }

            return Ok(new
            {
user.UserName,
user.Password

            });


        }
        [HttpGet("GetUserCounts")]
        public async Task <IActionResult>GetUserCounts()
        {

            var count =await _context.Users.CountAsync();

            return Ok(new { UserCount = count });


        }

    }
}
