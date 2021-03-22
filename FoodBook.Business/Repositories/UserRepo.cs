using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FoodBook.Business.Repositories.IRepositories;
using FoodBook.Common.DTOs.UserDTOs;
using FoodBook.DataAccess.Data;
using FoodBook.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FoodBook.Business.Repositories
{
    public class UserRepo : IUserRepo
    {

        #region Variables
        private readonly IMapper _mapper;
        private readonly FoodBookDbContext _context;
        private readonly IConfiguration _config;
        #endregion

        #region Constructor
        public UserRepo(IMapper mapper, FoodBookDbContext context, IConfiguration config)
        {
            _config = config;
            _mapper = mapper;
            _context = context;
        }
        #endregion

        #region UserMethods
        public async Task<UserDTO> CreateUser(UserDTO userToAdd, string password)
        {
            if(await DoesUserExist(userToAdd.Email))
            {
                return null;
            }
            byte[] passwordHash, passwordSalt;
            
            var user = _mapper.Map<UserDTO, User>(userToAdd);
            user.Email = user.Email.ToLower();
            user.CreateDate = DateTime.Now;
            user.UpdateDate = DateTime.Now;


            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<User, UserDTO>(user);
        }

        public async Task<LoginUserRespondDTO> LoginUser(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email.ToLower());
            if(user == null) return null;

            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;
            var token = CreateToken(user.Id.ToString(), user.Email);

            LoginUserRespondDTO userToReturn = new LoginUserRespondDTO {
                User = _mapper.Map<User, UserDTO>(user), 
                Token = token
            };
            return userToReturn;
        }

        public Task<bool> DeleteUser(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserDTO> GetUserById(string userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserDTO> UpdateUser(UserDTO userToUpdate)
        {
            throw new System.NotImplementedException();
        }
        #endregion


        #region PrivateMethods
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHashFromPassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHashFromPassword.Length; i++)
                {
                    if(passwordHash[i] != computedHashFromPassword[i]) return false;
                }
                return true;
            }
        }
        private string CreateToken(string userId, string userEmail)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, userEmail)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWTKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
       
        private async Task<bool> DoesUserExist(string userEmail){
            return await _context.Users.AnyAsync(x => x.Email == userEmail);
        }
        #endregion
    }
}