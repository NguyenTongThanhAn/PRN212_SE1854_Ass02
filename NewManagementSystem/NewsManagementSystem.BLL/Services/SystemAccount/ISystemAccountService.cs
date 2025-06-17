using NewsManagementSystem.BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.BLL.Services.SystemAccount
{
    public interface ISystemAccountService
    {
         Task<List<BusinessObject.Entities.SystemAccount>> GetSystemAccountsAsync();
        Task<List<BusinessObject.Entities.SystemAccount>> GetSystemAccountByNameAsync(string systemAccountName);
        Task CreateSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount);
        Task<BusinessObject.Entities.SystemAccount?> GetSystemAccountByIdAsync(short id);

        Task UpdateSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount);

        Task DeleteSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount);
        Task<BusinessObject.Entities.SystemAccount?> AuthenticateAsync(string email, string password);
    }
}
