using ETradingSystem1.EFCore;

using ETradingSystem1.Entities;

namespace ETradingSystem1.DAL
{
    public class BusinessOwnerDAL
    {
        ETradingSystemContext objETradingSystemContext = new ETradingSystemContext();

        public void CreateBusinessOwner(BusinessOwner objBusinessOwner)
        {
            objETradingSystemContext.Add(objBusinessOwner);
            objETradingSystemContext.SaveChanges();
        }

        public void UpdateBusinessOwner(BusinessOwner objBusinessOwner)
        {
            objETradingSystemContext.Entry(objBusinessOwner).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            objETradingSystemContext.SaveChanges();
        }

        public void DeleteBusinessOwner(int id)
        {
            BusinessOwner objBusinessOwner = objETradingSystemContext.BusinessOwners.Find(id);
            objETradingSystemContext.Remove(objBusinessOwner);
            objETradingSystemContext.SaveChanges();
        }

        public BusinessOwner GetBusinessOwner(int id)
        {
            BusinessOwner objBusinessOwner = objETradingSystemContext.BusinessOwners.Find(id);
            return objBusinessOwner;
        }

        public IEnumerable<BusinessOwner> GetBusinessOwners()
        {
            return objETradingSystemContext.BusinessOwners;
        }

    }
}
