using Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public class IAuthService
    {
        Task<LoginResponse> AuthenticateAsync(string username, string password);
    }
}
