using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using catedra3.src.models;

namespace catedra3.src.interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}