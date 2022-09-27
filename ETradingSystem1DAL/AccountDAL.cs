using ETradingSystem1.EFCore;

using ETradingSystem1.Entities;

namespace ETradingSystem1.DAL
{
    public class AccountDAL
    {
        ETradingSystemContext objETradingSystemContext = new ETradingSystemContext();

        public void CreateAccount(Account objAccount)
        {
            objETradingSystemContext.Add(objAccount);
            objETradingSystemContext.SaveChanges();
        }

        public void UpdateAccount(Account objAccount)
        {
            objETradingSystemContext.Entry(objAccount).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            objETradingSystemContext.SaveChanges();
        }

        public void DeleteAccount(int id)
        {
            Account objAccount = objETradingSystemContext.Accounts.Find(id);
            objETradingSystemContext.Remove(objAccount);
            objETradingSystemContext.SaveChanges();
        }

        public Account GetAccount(int id)
        {
            Account objAccount = objETradingSystemContext.Accounts.Find(id);
            return objAccount;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return objETradingSystemContext.Accounts;
        }

    }
}
