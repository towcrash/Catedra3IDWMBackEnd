using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using catedra3.src.data;
using catedra3.src.dtos;
using catedra3.src.Helpers;
using catedra3.src.interfaces;
using catedra3.src.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace catedra3.src.repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDBContext _context;

        public AuthRepository(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, ApplicationDBContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<NewUserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.Password))
            {
                throw new ArgumentException("Password is required");
            }
            if (await EmailExistsAsync(registerDto.Email))
            {
                throw new Exception("A product with the same name and type already exists.");
            }
            var user = new AppUser
            {
                UserName = RandomStringGenerator.Generate(),
                Email = registerDto.Email
            };

            var createUser = await _userManager.CreateAsync(user, registerDto.Password);

            if (!createUser.Succeeded)
            {
                throw new InvalidOperationException(string.Join("; ", createUser.Errors.Select(e => e.Description)));
            }


            // El usuario es creado exitosamente
            return new NewUserDto
            {
                Email = user.Email,
                Token = await _tokenService.CreateToken(user)
            };
        }
        public async Task<NewUserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid user or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid user or password");
            }

            return new NewUserDto
            {
                id = user.Id,
                Email = user.Email!,
                Token = await _tokenService.CreateToken(user)
            };
        }
        private async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email); 
        }
    }
}