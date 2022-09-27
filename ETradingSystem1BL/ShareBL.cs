using ETradingSystem1.DAL;
using ETradingSystem1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETradingSystem1.BL
{
    public class ShareBL
    {
        ShareDAL objShareDAL = new ShareDAL();

        public void CreateShare(Share objShare)
        {
            objShareDAL.CreateShare(objShare);
        }

        public void UpdateShare(Share objShare)
        {
            objShareDAL.UpdateShare(objShare);
        }

        public void DeleteShare(int id)
        {
            objShareDAL.DeleteShare(id);
        }

        public Share GetShare(int id)
        {
            Share objShare = objShareDAL.GetShare(id);
            return objShare;
        }

        public IEnumerable<Share> GetShares()
        {
            return objShareDAL.GetShares();
        }
    }
}
