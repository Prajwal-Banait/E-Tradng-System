using ETradingSystem1.BL;
using ETradingSystem1.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETradingSystem1.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class customerWebAPIController : ControllerBase
    {
        private readonly CustomerBL customerBL = new CustomerBL();

        public customerWebAPIController()
        {

        }

        // GET: api/CustomerWebAPI
        [HttpGet]
        [Authorize(Roles = "Admin,Customer")]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
           
            return new ActionResult<IEnumerable<Customer>>(customerBL.GetCustomers());
        }

        // GET: api/CustomerWebAPI/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Customer")]
        public ActionResult<Customer> GetCustomer(int id)
        {
            var customer = customerBL.GetCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/CustomerWebAPI/5

        [HttpPut("{id}")]
        [Authorize(Roles = "Customer")]
        public IActionResult PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            try
            {
                customerBL.UpdateCustomer(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CustomerWebAPI
        
        [HttpPost]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            
            try
            {
                customerBL.CreateCustomer(customer);
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.CustomerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/CustomerWebAPI/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public ActionResult<Customer> DeleteCustomer(int id)
        {
           
            var customer = customerBL.GetCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }

            customerBL.DeleteCustomer(customer.CustomerId);

            
            return customer;
        }

        private bool CustomerExists(int id)
        {
            if (customerBL.GetCustomer(id) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
