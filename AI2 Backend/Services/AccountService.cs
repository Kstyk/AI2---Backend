﻿using AI2_Backend.Entities;
using AI2_Backend.Models;
using AI2_Backend.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AI2_Backend.Services
{
    public class AccountService : IAccountService
    {
        private AIDbContext _context;
        private IPasswordHasher<User> _passwordHasher;
        private AuthenticationSettings _settings;
        private readonly IMapper _mapper;
        private IUserContextService _userContextService;

        public AccountService(AIDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IMapper mapper, IUserContextService userContext)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _settings = authenticationSettings;
            _mapper = mapper;
            _userContextService = userContext;
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = _mapper.Map<User>(dto);
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
            newUser.Password = hashedPassword;
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }

        public string GenerateJwt(LoginUserDto loginUserDto)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Email == loginUserDto.Email);

            if (user is null)
            {
                throw new Exception();
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginUserDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new Exception();
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("Id", $"{user.Id}"),
                new Claim("Email", $"{user.Email}"),
                
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_settings.JwtExpireDays);

            var token = new JwtSecurityToken(_settings.JwtIssuer, _settings.JwtIssuer, claims, expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();


            return tokenHandler.WriteToken(token);
        }
    }
}
