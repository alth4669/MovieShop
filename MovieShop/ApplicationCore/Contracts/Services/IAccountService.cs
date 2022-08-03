using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Services
{
    public interface IAccountService
    {
        Task<bool> CreateUser(UserRegisterModel model);
        Task<ActiveUserModel> ValidateUser(UserLoginModel model);
        Task<bool> EmailExists(string email);
    }
}
