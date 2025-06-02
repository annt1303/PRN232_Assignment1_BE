
using BLL.Models;
using BLL.Models.Request;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterface
{
    public interface ISystemAccountService
    {
        //Task<SystemAccountDTO?> Login(string email, string password);

        Task<LoginResultDTO?> Login(string email, string password);

		Task<List<SystemAccountDTO>?> GetAllAccounts();
        Task<SystemAccountDTO?> GetAccountById(short id);
        Task<SystemAccountDTO> CreateAccount(SystemAccountRequest accountDto);
        Task<bool> UpdateAccount(SystemAccountDTO accountDto);
        Task<bool> UpdateProfileStaff(SystemAccountDTO accountDto);
        Task<bool> DeleteAccount(short id);
        Task<List<SystemAccountDTO>?> SearchAccountsByNameAsync(string name);
    }
}
