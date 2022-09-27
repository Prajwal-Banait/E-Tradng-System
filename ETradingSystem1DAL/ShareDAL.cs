using ETradingSystem1.EFCore;

using ETradingSystem1.Entities;

namespace ETradingSystem1.DAL
{
    public class ShareDAL
    {
        ETradingSystemContext objETradingSystemContext = new ETradingSystemContext();

        public void CreateShare(Share objShare)
        {
            objETradingSystemContext.Add(objShare);
            objETradingSystemContext.SaveChanges();
        }

        public void UpdateShare(Share objShare)
        {
            objETradingSystemContext.Entry(objShare).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            objETradingSystemContext.SaveChanges();
        }

        public void DeleteShare(int id)
        {
            Share objShare = objETradingSystemContext.Shares.Find(id);
            objETradingSystemContext.Remove(objShare);
            objETradingSystemContext.SaveChanges();
        }

        public Share GetShare(int id)
        {
            Share objShare = objETradingSystemContext.Shares.Find(id);
            return objShare;
        }

        public IEnumerable<Share> GetShares()
        {
            return objETradingSystemContext.Shares;
        }

    }
}
