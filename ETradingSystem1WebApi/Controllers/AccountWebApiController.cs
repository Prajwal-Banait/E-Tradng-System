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
    public class accountWebAPIController : ControllerBase
    {
        private readonly AccountBL accountBL = new AccountBL();

        public accountWebAPIController()
        {

        }

        // GET: api/AccountWebAPI
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult<IEnumerable<Account>> GetAccounts()
        {
           
            return new ActionResult<IEnumerable<Account>>(accountBL.GetAccounts());
        }

        // GET: api/AccountWebAPI
        [HttpGet("{id}")]
        [Authorize(Roles = "Customer")]
        public ActionResult<Account> GetAccount(int id)
        {
            var account = accountBL.GetAccount(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/AccountWebAPI/5
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Customer")]
        public IActionResult PutAccount(int id, Account account)
        {
            if (id != account.AccountId)
            {
                return BadRequest();
            }

            try
            {
                accountBL.UpdateAccount(account);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
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

        // POST: api/AccounttWebAPI
        
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
           
            try
            {
                accountBL.CreateAccount(account);
            }
            catch (DbUpdateException)
            {
                if (AccountExists(account.AccountId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
        }

        // DELETE: api/AccounttWebAPI/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer")]
        public ActionResult<Account> DeleteAccount(int id)
        {
           
            var account = accountBL.GetAccount(id);
            if (account == null)
            {
                return NotFound();
            }

            accountBL.DeleteAccount(account.AccountId);

            
            return account;
        }

        private bool AccountExists(int id)
        {
            if (accountBL.GetAccount(id) != null)
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
