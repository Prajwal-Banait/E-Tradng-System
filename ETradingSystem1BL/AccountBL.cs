using ETradingSystem1.DAL;
using ETradingSystem1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETradingSystem1.BL
{
    public class AccountBL
    {
        AccountDAL objAccountDAL = new AccountDAL();

        public void CreateAccount(Account objAccount)
        {
            objAccountDAL.CreateAccount(objAccount);
        }

        public void UpdateAccount(Account objAccount)
        {
            objAccountDAL.UpdateAccount(objAccount);
        }

        public void DeleteAccount(int id)
        {
            objAccountDAL.DeleteAccount(id);
        }

        public Account GetAccount(int id)
        {
            Account objAccount = objAccountDAL.GetAccount(id);
            return objAccount;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return objAccountDAL.GetAccounts();
        }
    }
}
