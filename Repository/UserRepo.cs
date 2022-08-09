using Microsoft.IdentityModel.Tokens;
using AuthService.Models;
using AuthService.Models.DTOs;
using AuthService.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AuthService.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _db;
        public UserRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        #region Private        
        private static string GetJwtTokenString()
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Secret key string"));
                var SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(

                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: SigningCredentials
                    );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return tokenString;
            }
            catch (Exception ex)
            {
                throw new Exception("AuthService.Repository.GetJwtTokenString", ex);
            }
        }
        private int addUserPvt(UserDto UserData)
        {
            try {
                User user = new User();
                user.UserName = UserData.UserName;
                user.Password = UserData.Password;
                user.Name = UserData.Name;
                user.PhNumber = UserData.PhNumber;

                _db.user.Add(user);

                int status = _db.SaveChanges();
                return status;
            }
            catch(Exception ex)
            {
                throw new Exception("AuthService.Repository.addUserPvt", ex);
            }
            
        }
        private UserDto AuthenticationPvt(string username, string password)
        {
            try
            {
                User userData = new User();
                UserDto user = new UserDto();
                userData = _db.user.FirstOrDefault(x => x.UserName.ToLower() == username.ToLower() && x.Password == password);
                if (userData != null)
                {
                    //user.UserName = userData.UserName;
                    user.Name = userData.Name;
                    user.PhNumber = userData.PhNumber;
                    user.token = GetJwtTokenString();

                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("AuthService.Repository.AuthenticationPvt", ex);
            }
            
        }

        #endregion

        #region Public       

        public int addUser(UserDto UserData)
        {
            try
            {
                if (userExist(UserData.UserName))
                {
                    return -1;
                }

                int status = addUserPvt(UserData);
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception("AuthService.Repository.addUser", ex);
            }

        }
        public UserDto authenticate(string username, string password)
        {
            try
            {
                if (!userExist(username))
                {
                    return null;
                }
                return AuthenticationPvt(username, password);
            }
            catch (Exception ex)
            {

                throw new Exception("AuthService.Repository.authenticate", ex);
            }
        }
        public bool userExist(string username)
        {
            try
            {
                var user = _db.user.FirstOrDefault(x => x.UserName == username);
                if (user == null)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("AuthService.Repository.userExist", ex);
            }
        }          
        #endregion

    }
}
