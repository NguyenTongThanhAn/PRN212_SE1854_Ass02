using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsManagementSystem.DAL.SystemAccount
{
    public interface ISystemAccountRepo
    {
        Task<List<BusinessObject.Entities.SystemAccount>> GetSystemAccountsAsync();
        Task<List<BusinessObject.Entities.SystemAccount>> GetSystemAccountByNameAsync(string systemAccountName);
        Task CreateSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount);
        Task<BusinessObject.Entities.SystemAccount?> GetSystemAccountByIdAsync(short id);

        Task UpdateSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount);

        Task DeleteSystemAccountAsync(BusinessObject.Entities.SystemAccount systemAccount);
        Task<BusinessObject.Entities.SystemAccount?> GetByEmailAndPasswordAsync(string email, string password);
    }
}
