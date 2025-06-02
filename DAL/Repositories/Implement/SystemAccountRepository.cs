using DAL.Core;
using DAL.Models;
using DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implement
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly FUNewsManagementContext _context;

        public SystemAccountRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<SystemAccount>> GetAllAccountsAsync(int page, int size)
        {
            var query = _context.SystemAccounts.AsNoTracking();
            return await PaginatedList<SystemAccount>.CreateAsync(query, page, size);
        }


        public async Task<PaginatedList<SystemAccount>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.SystemAccounts.AsNoTracking();
            return await PaginatedList<SystemAccount>.CreateAsync(query, pageNumber, pageSize);
        }



        public async Task<SystemAccount?> GetAccountById(short id)
        {
            return await _context.SystemAccounts.AsNoTracking().FirstOrDefaultAsync(a => a.AccountId == id);
        }

        public async Task<SystemAccount?> Login(string email, string password)
        {
            return await _context.SystemAccounts
                        .FirstOrDefaultAsync(x => x.AccountEmail == email && x.AccountPassword == password);
        }

        public async Task<SystemAccount> CreateAccount(SystemAccount account)
        {
            _context.SystemAccounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<bool> UpdateAccount(SystemAccount account)
        {
            var existingAccount = await _context.SystemAccounts.FindAsync(account.AccountId);
            if (existingAccount == null)
                return false;

            _context.Entry(existingAccount).CurrentValues.SetValues(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccount(short id)
        {
            var account = await _context.SystemAccounts.FindAsync(id);
            if (account == null)
                return false;

            _context.SystemAccounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SystemAccount>> SearchAccountsByNameAsync(string name)
        {

            return await _context.SystemAccounts
                .Where(a => a.AccountName.Contains(name)).AsNoTracking().ToListAsync();

        }

        public async Task<short> GenerateId()
        {
            SystemAccount user = await _context.SystemAccounts.OrderByDescending(x => x.AccountId).FirstOrDefaultAsync();
            return (short)(user == null ? 1 : user.AccountId + 1);
        }
    }
}
