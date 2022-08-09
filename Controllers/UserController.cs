using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.Models.DTOs;
using AuthService.Repository.IRepository;
using System;
using System.Net.Http;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _IUserRepo;
        public UserController(IUserRepo userRepo)
        {
            _IUserRepo = userRepo;
        }
        [HttpPost("UserSignUp")]
        [AllowAnonymous]
        public IActionResult userSignUp([FromBody] UserDto userData)
        {
            try
            {
                if (!string.IsNullOrEmpty(userData.UserName) && !string.IsNullOrEmpty(userData.Password))
                {
                    int saveStatus = _IUserRepo.addUser(userData);
                    if (saveStatus < 0)
                    {
                        return BadRequest(new { userData = userData, message = "User Name is already in Use!" });
                    }
                    else if (saveStatus == 0)
                    {
                        return BadRequest(new { userData = userData, message = "An error occured while saving" });
                    }
                    else
                        return Ok(userData);
                }
                else
                {
                    return BadRequest(new { userData = userData, message = "Please Enter UserName and Password" });
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [AllowAnonymous]
        [HttpPost("UserLogIn")]
        public IActionResult userLogIn([FromBody] UserDto userDto)
        {
            try
            {
                if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password))
                {
                    return BadRequest(new { message = "Please enter Username/Password" });
                }
                UserDto user = _IUserRepo.authenticate(userDto.UserName, userDto.Password);

                if (user == null || string.IsNullOrEmpty(user.token))
                {
                    //return BadRequest(new { message = "Invalid UserName/ Password" });
                    return Unauthorized(new { message = "Invalid UserName/ Password" });
                }
                return Ok(new { Token = user.token, Name = user.Name }) ;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
