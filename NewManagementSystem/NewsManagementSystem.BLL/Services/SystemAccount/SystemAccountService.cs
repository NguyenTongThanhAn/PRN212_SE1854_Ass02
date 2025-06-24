using Microsoft.Extensions.Configuration;
using NewsManagementSystem.DAL.SystemAccount;
using System.Collections.Generic;
using System.Threading.Tasks;
using BO = NewsManagementSystem.BusinessObject.Entities;

namespace NewsManagementSystem.BLL.Services.SystemAccount
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepo _repository;
        private readonly IConfiguration _config;

        public SystemAccountService(ISystemAccountRepo repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public async Task<List<BO.SystemAccount>> GetSystemAccountsAsync()
        {
            return await _repository.GetSystemAccountsAsync();
        }

        public async Task<List<BO.SystemAccount>> GetSystemAccountByNameAsync(string systemAccountName)
        {
            return await _repository.GetSystemAccountByNameAsync(systemAccountName);
        }

        public async Task CreateSystemAccountAsync(BO.SystemAccount systemAccount)
        {
            await _repository.CreateSystemAccountAsync(systemAccount);
        }

        public async Task<BO.SystemAccount?> GetSystemAccountByIdAsync(short id)
        {
            return await _repository.GetSystemAccountByIdAsync(id);
        }

        public async Task UpdateSystemAccountAsync(BO.SystemAccount systemAccount)
        {
            await _repository.UpdateSystemAccountAsync(systemAccount);
        }

        public async Task DeleteSystemAccountAsync(BO.SystemAccount systemAccount)
        {
            await _repository.DeleteSystemAccountAsync(systemAccount);
        }
        
        public Task<List<BO.SystemAccount>> SearchAsync(string keyword) => _repository.SearchAsync(keyword);


        public async Task<BO.SystemAccount?> AuthenticateAsync(string email, string password)
        {
            var adminEmail = _config["AdminAccount:Email"];
            var adminPass = _config["AdminAccount:Password"];

            if (email == adminEmail && password == adminPass)
            {
                return new BO.SystemAccount
                {
                    AccountEmail = email,
                    AccountName = "Admin",
                    AccountRole = 0
                };
            }

            return await _repository.GetByEmailAndPasswordAsync(email, password);
        }
    }
}
