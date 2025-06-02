
using BLL.Models;
using BLL.Models.Request;
using BLL.ServiceInterface;
using DAL.Models;
using DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceImp
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _systemAccountRepository;

        private readonly INewsArticleRepository _newsArticleRepository;

        public SystemAccountService(ISystemAccountRepository systemAccountRepository, INewsArticleRepository newsArticleRepository)
        {
            _systemAccountRepository = systemAccountRepository;
            _newsArticleRepository = newsArticleRepository;
        }

        public async Task<SystemAccountDTO> Login(string email, string password)
        {
            SystemAccount? systemAccount = await _systemAccountRepository.Login(email, password);
            if (systemAccount == null) return null;
            return MapToDTO(systemAccount);
        }

        public async Task<List<SystemAccountDTO>?> GetAllAccounts()
        {
            List<SystemAccount>? systemAccounts = await _systemAccountRepository.GetAllAccounts();
            if (systemAccounts == null) return null;
            List<SystemAccountDTO> systemAccountDTOs = new List<SystemAccountDTO>();
            foreach (SystemAccount item in systemAccounts)
            {
                systemAccountDTOs.Add(MapToDTO(item));
            }
            return systemAccountDTOs;
        }

        public async Task<SystemAccountDTO?> GetAccountById(short id)
        {
            SystemAccount? systemAccount = await _systemAccountRepository.GetAccountById(id);
            return systemAccount != null ? MapToDTO(systemAccount) : null;
        }

        public async Task<SystemAccountDTO> CreateAccount(SystemAccountRequest accountDto)
        {
            short AccountId = await _systemAccountRepository.GenerateId();
            var account = new SystemAccount
            {
                AccountId = AccountId,
                AccountName = accountDto.AccountName!,
                AccountEmail = accountDto.AccountEmail!,
                AccountRole = accountDto.AccountRole,
                AccountPassword = accountDto.AccountPassword!
            };

            var createdAccount = await _systemAccountRepository.CreateAccount(account);
            return MapToDTO(createdAccount);
        }
        public async Task<bool> UpdateAccount(SystemAccountDTO accountDto)
        {
            var existingAccount = await _systemAccountRepository.GetAccountById(accountDto.AccountId);
            if (existingAccount == null) return false;

            existingAccount.AccountName = accountDto.AccountName!;
            if (!string.IsNullOrWhiteSpace(accountDto.AccountPassword))
            {
                existingAccount.AccountPassword = accountDto.AccountPassword;
            }

            return await _systemAccountRepository.UpdateAccount(existingAccount);
        }

        public async Task<bool> DeleteAccount(short id)
        {
            // Check if the account is associated with any news articles
            List<NewsArticle>? articles = await _newsArticleRepository.GetArticlesByStaffIdAsync(id);
            if (articles != null && articles.Count > 0)
            {
                throw new Exception("Account is already created any news article, cannot delete. ");
            }

            return await _systemAccountRepository.DeleteAccount(id);
        }

        private SystemAccountDTO MapToDTO(SystemAccount systemAccount)
        {
            return new SystemAccountDTO
            {
                AccountId = systemAccount.AccountId,
                AccountName = systemAccount.AccountName,
                AccountEmail = systemAccount.AccountEmail,
                AccountRole = systemAccount.AccountRole
            };
        }

        /// <summary>
        /// search account by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<SystemAccountDTO>?> SearchAccountsByNameAsync(string name)
        {
            if (name == null)
            {
                return await GetAllAccounts();
            }
            List<SystemAccount>? systemAccounts = await _systemAccountRepository.SearchAccountsByNameAsync(name);
            if (systemAccounts == null) return null;
            List<SystemAccountDTO> systemAccountDTOs = new List<SystemAccountDTO>();
            foreach (SystemAccount item in systemAccounts)
            {
                systemAccountDTOs.Add(MapToDTO(item));
            }
            return systemAccountDTOs;
        }

        /// <summary>
        /// update profile staff
        /// </summary>
        /// <param name="accountDto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateProfileStaff(SystemAccountDTO accountDto)
        {
            var existingAccount = await _systemAccountRepository.GetAccountById(accountDto.AccountId);

            if (existingAccount == null) return false;

            existingAccount.AccountName = accountDto.AccountName!;

            if (!string.IsNullOrWhiteSpace(accountDto.AccountPassword))
            {
                existingAccount.AccountPassword = accountDto.AccountPassword;
            }

            return await _systemAccountRepository.UpdateAccount(existingAccount);
        }

    }
}
