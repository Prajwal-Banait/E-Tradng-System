using ETradingSystem1.DAL;
using ETradingSystem1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETradingSystem1.BL
{
    public class BusinessOwnerBL
    {
        BusinessOwnerDAL objBusinessOwnerDAL = new BusinessOwnerDAL();

        public void CreateBusinessOwner(BusinessOwner objBusinessOwner)
        {
            objBusinessOwnerDAL.CreateBusinessOwner(objBusinessOwner);
        }

        public void UpdateBusinessOwner(BusinessOwner objBusinessOwner)
        {
            objBusinessOwnerDAL.UpdateBusinessOwner(objBusinessOwner);
        }

        public void DeleteBusinessOwner(int id)
        {
            objBusinessOwnerDAL.DeleteBusinessOwner(id);
        }

        public BusinessOwner GetBusinessOwner(int id)
        {
            BusinessOwner objBusinessOwner = objBusinessOwnerDAL.GetBusinessOwner(id);
            return objBusinessOwner;
        }

        public IEnumerable<BusinessOwner> GetBusinessOwners()
        {
            return objBusinessOwnerDAL.GetBusinessOwners();
        }
    }
}
