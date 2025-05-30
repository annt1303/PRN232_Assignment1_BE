using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interface
{
    public interface ISystemAccountRepository
    {
        Task<List<SystemAccount>?> GetAllAccounts();
        Task<SystemAccount?> GetAccountById(short id);
        Task<SystemAccount?> Login(string email, string password);
        Task<SystemAccount> CreateAccount(SystemAccount account);
        Task<bool> UpdateAccount(SystemAccount account);
        Task<bool> DeleteAccount(short id);
        Task<List<SystemAccount>> SearchAccountsByNameAsync(string name);
        Task<short> GenerateId();
    }
}
