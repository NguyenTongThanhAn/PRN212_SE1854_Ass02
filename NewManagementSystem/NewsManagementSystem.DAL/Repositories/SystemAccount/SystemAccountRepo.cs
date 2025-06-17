using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsManagementSystem.DAL.SystemAccount
{
    public class SystemAccountRepo : ISystemAccountRepo
    {
        private readonly FUNewsManagementContext _context;

        public SystemAccountRepo(FUNewsManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<BusinessObject.Entities.SystemAccount?> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(acc => acc.AccountEmail == email && acc.AccountPassword == password);
        }

        public async Task CreateSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount)
        {
            await _context.SystemAccounts.AddAsync(systemAccount);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount)
        {
            //    _context.NewsArticles.Remove(article);
            //}

            //_context.SystemAccounts.Remove(systemAccount);
            //await _context.SaveChangesAsync();

            var account = await _context.SystemAccounts.Include(a=>a.NewsArticleCreatedBies).FirstOrDefaultAsync(a => a.AccountId == systemAccount.AccountId);
            if (account == null)
            {
                throw new KeyNotFoundException($"System account with ID {systemAccount.AccountId} not found.");
            }
            account.NewsArticleCreatedBies.Clear(); 
            _context.SystemAccounts.Remove(account); 
            await _context.SaveChangesAsync();
        }

        public async Task<BusinessObject.Entities.SystemAccount?> GetSystemAccountByIdAsync(short id)
        {
            return await _context.SystemAccounts.FindAsync(id);
        }

        public async Task<List<BusinessObject.Entities.SystemAccount>> GetSystemAccountByNameAsync(string systemAccountName)
        {
            return await _context.SystemAccounts.Where(x => x.AccountName.ToLower().Contains(systemAccountName.ToLower())).OrderByDescending(x => x.AccountId).ToListAsync();
        }

        public async Task<List<BusinessObject.Entities.SystemAccount>> GetSystemAccountsAsync()
        {
            return await _context.SystemAccounts.OrderByDescending(x => x.AccountId).ToListAsync();
        }

        public async Task UpdateSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount)
        {
            var existingAccount = await _context.SystemAccounts.FindAsync(systemAccount.AccountId);
            if (existingAccount == null)
            {
                throw new KeyNotFoundException($"System account with ID {systemAccount.AccountId} not found.");
            }

            existingAccount.AccountName = systemAccount.AccountName;
            existingAccount.AccountPassword = systemAccount.AccountPassword;
            existingAccount.AccountEmail = systemAccount.AccountEmail;
            existingAccount.AccountRole = systemAccount.AccountRole;

            await _context.SaveChangesAsync();
        }
    }
}
