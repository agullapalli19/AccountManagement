using AccountManagementAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AccountManagementAPI.Repository
{
    public interface IAccountsRepo
    {
        public List<Account> GetAccounts();

        public Account? GetAccountByID(int id);
        public Account? Transfer(TransferRequest request);
    }
}
