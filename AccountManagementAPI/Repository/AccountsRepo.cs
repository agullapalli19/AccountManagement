using AccountManagementAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
namespace AccountManagementAPI.Repository
{
    public class AccountsRepo: IAccountsRepo
    {
        private readonly AccountDBContext _dbContext;
   
        public AccountsRepo(AccountDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Account> GetAccounts()
        {
            return _dbContext.Accounts.ToList();
        }

        public Account? GetAccountByID(int id)
        {
            return _dbContext.Accounts.Where(account => account.AccountId == id).FirstOrDefault();
        }

        public Account? Transfer(TransferRequest request)
        {
            
            var sourceAccount = _dbContext.Accounts.Where(account => account.AccountNumber == request.SourceAccountNumber && account.AccountHolderId == request.PersonId).FirstOrDefault();
            var targetAccount = _dbContext.Accounts.Where(account => account.AccountNumber == request.TargetAccountNumber).FirstOrDefault();

            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (sourceAccount is not null && targetAccount is not null)
                    {
                        sourceAccount.Balance = sourceAccount.Balance - request.Amount;
                        targetAccount.Balance = targetAccount.Balance + request.Amount;


                        Transaction transaction = new Transaction()
                        {
                            PersonId = request.PersonId,
                            SourceAccountId = _dbContext.Accounts.Where(account => account.AccountNumber == request.SourceAccountNumber).FirstOrDefault().AccountId,
                            TargetAccountId = _dbContext.Accounts.Where(account => account.AccountNumber == request.TargetAccountNumber).FirstOrDefault().AccountId,
                           
                            Amount = request.Amount,
                            CreatedOn = DateTime.Now
                        };
                        _dbContext.Transactions.Add(transaction);
                        _dbContext.SaveChanges();

                        dbContextTransaction.Commit();

                        var updatedSourceAccount = _dbContext.Accounts.Where(account => account.AccountNumber == request.SourceAccountNumber).FirstOrDefault();
                        return updatedSourceAccount;

                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return null;
                   //Logger can be used to log exceptions
                }
            }

        }

    }
}
