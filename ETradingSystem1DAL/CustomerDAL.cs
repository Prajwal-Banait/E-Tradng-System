using ETradingSystem1.EFCore;

using ETradingSystem1.Entities;

namespace ETradingSystem1.DAL
{
    public class CustomerDAL
    {
        ETradingSystemContext objETradingSystemContext = new ETradingSystemContext();

        public void CreateCustomer(Customer objCustomer)
        {
            objETradingSystemContext.Add(objCustomer);
            objETradingSystemContext.SaveChanges();
        }

        public void UpdateCustomer(Customer objCustomer)
        {
            objETradingSystemContext.Entry(objCustomer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            objETradingSystemContext.SaveChanges();
        }

        public void DeleteCustomer(int id)
        {
            Customer objCustomer = objETradingSystemContext.Customers.Find(id);
            objETradingSystemContext.Remove(objCustomer);
            objETradingSystemContext.SaveChanges();
        }

        public Customer GetCustomer(int id)
        {
            Customer objCustomer = objETradingSystemContext.Customers.Find(id);
            return objCustomer;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return objETradingSystemContext.Customers;
        }

    }
}
