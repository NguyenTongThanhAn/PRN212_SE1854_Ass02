using Microsoft.EntityFrameworkCore;
using NewsManagementSystem.DAL.DBContext;
using NewsManagementSystem.DAL.SystemAccount;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BO = NewsManagementSystem.BusinessObject.Entities;

namespace NewsManagementSystem.DAL.Repositories.SystemAccount
{
    public class SystemAccountRepo : ISystemAccountRepo
    {
        private readonly FUNewsManagementContext _context;

        public SystemAccountRepo(FUNewsManagementContext context)
        {
            _context = context;
        }

        public async Task<List<BO.SystemAccount>> GetSystemAccountsAsync()
        {
            return await _context.SystemAccounts.ToListAsync();
        }

        public async Task<List<BO.SystemAccount>> GetSystemAccountByNameAsync(string systemAccountName)
        {
            return await _context.SystemAccounts
                .Where(a => a.AccountName.Contains(systemAccountName))
                .ToListAsync();
        }

        public async Task CreateSystemAccountAsync(BO.SystemAccount systemAccount)
        {
            await _context.SystemAccounts.AddAsync(systemAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<BO.SystemAccount?> GetSystemAccountByIdAsync(short id)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(a => a.AccountId == id);
        }

        public async Task UpdateSystemAccountAsync(BO.SystemAccount systemAccount)
        {
            _context.SystemAccounts.Update(systemAccount);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSystemAccountAsync(BO.SystemAccount systemAccount)
        {
            _context.SystemAccounts.Remove(systemAccount);
            await _context.SaveChangesAsync();
        }

        public async Task<BO.SystemAccount?> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(a => a.AccountEmail == email && a.AccountPassword == password);
        }
    }
}
