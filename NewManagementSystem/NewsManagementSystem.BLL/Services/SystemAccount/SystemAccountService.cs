using Microsoft.Extensions.Configuration;
using NewsManagementSystem.DAL.SystemAccount;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsManagementSystem.BLL.Services.SystemAccount
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepo _systemAccountRepo;
        private readonly IConfiguration _config;

        public SystemAccountService(ISystemAccountRepo systemAccountRepo, IConfiguration config)
        {
            _systemAccountRepo = systemAccountRepo;
            _config = config;
        }

        public async Task<BusinessObject.Entities.SystemAccount?> AuthenticateAsync(string email, string password)
        {
            
            var adminEmail = _config["AdminAccount:Email"];
            var adminPassword = _config["AdminAccount:Password"];
            var adminRole = _config["AdminAccount:Role"];
            if (email == adminEmail && password == adminPassword)
            {
                return new BusinessObject.Entities.SystemAccount
                {
                    AccountEmail = adminEmail,
                    AccountPassword = adminPassword,
                    AccountRole =  Convert.ToInt32(adminRole), // Admin
                    AccountName = "Administrator"
                };
            }

            
            return await _systemAccountRepo.GetByEmailAndPasswordAsync(email, password);
        }

        public async Task CreateSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount)
        {
            await _systemAccountRepo.CreateSystemAccountAsync(systemAccount);
        }

        public async Task DeleteSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount)
        {
            await _systemAccountRepo.DeleteSystemAccountAsync(systemAccount);
        }

        public async Task<BusinessObject.Entities.SystemAccount?> GetSystemAccountByIdAsync(short id)
        {
            return await _systemAccountRepo.GetSystemAccountByIdAsync(id);
        }

        public async Task<List<BusinessObject.Entities.SystemAccount>> GetSystemAccountByNameAsync(
            string systemAccountName)
        {
            return await _systemAccountRepo.GetSystemAccountByNameAsync(systemAccountName);
        }

        public async Task<List<BusinessObject.Entities.SystemAccount>> GetSystemAccountsAsync()
        {
            return await _systemAccountRepo.GetSystemAccountsAsync();
        }

        public async Task UpdateSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount)
        {
            await _systemAccountRepo.UpdateSystemAccountAsync(systemAccount);
        }
    }
}
