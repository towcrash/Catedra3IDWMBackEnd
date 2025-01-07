using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catedra3.src.dtos;

namespace catedra3.src.interfaces
{
    public interface IAuthRepository
    {
        
        Task<NewUserDto> RegisterAsync(RegisterDto registerDto);
        Task<NewUserDto> LoginAsync(LoginDto loginDto);
    }
}