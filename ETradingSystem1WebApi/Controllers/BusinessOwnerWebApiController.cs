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
    public class businessOwnerWebAPIController : ControllerBase
    {
        private readonly BusinessOwnerBL businessOwnerBL = new BusinessOwnerBL();

        public businessOwnerWebAPIController()
        {

        }

        // GET: api/BusinessOwnerWebAPI
        [HttpGet]
       [Authorize(Roles="Admin,BusinessOwner")]
        public ActionResult<IEnumerable<BusinessOwner>> GetBusinessOwners()
        {
           
            return new ActionResult<IEnumerable<BusinessOwner>>(businessOwnerBL.GetBusinessOwners());
        }

        // GET: api/BusinessOwnerWebAPI/5
        [HttpGet("{id}")]
        [Authorize(Roles = "BusinessOwner")]
        public ActionResult<BusinessOwner> GetBusinessOwner(int id)
        {
            var businessOwner = businessOwnerBL.GetBusinessOwner(id);

            if (businessOwner == null)
            {
                return NotFound();
            }

            return businessOwner;
        }

        // PUT: api/BusinessOwnerWebAPI/5

        [HttpPut("{id}")]
        [Authorize(Roles = "BusinessOwner")]
        
        public IActionResult PutBusinessOwner(int id, BusinessOwner businessOwner)
        {
            if (id != businessOwner.BusinessOwnerId)
            {
                return BadRequest();
            }

            try
            {
                businessOwnerBL.UpdateBusinessOwner(businessOwner);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessOwnerExists(id))
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

        // POST: api/BusinessOwnerWebAPI

        [HttpPost]
        [Authorize(Roles = "Admin,BusinessOwner")]
        public async Task<ActionResult<BusinessOwner>> PostBusinessOwner(BusinessOwner businessOwner)
        {
           
            try
            {
                businessOwnerBL.CreateBusinessOwner(businessOwner);
            }
            catch (DbUpdateException)
            {
                if (BusinessOwnerExists(businessOwner.BusinessOwnerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBusinessOwner", new { id = businessOwner.BusinessOwnerId }, businessOwner);
        }

        // DELETE: api/BusinessOwnerWebAPI/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,BusinessOwner")]
        public ActionResult<BusinessOwner> DeleteBusinessOwner(int id)
        {
           
            var businessOwner = businessOwnerBL.GetBusinessOwner(id);
            if (businessOwner == null)
            {
                return NotFound();
            }

            businessOwnerBL.DeleteBusinessOwner(businessOwner.BusinessOwnerId);

            
            return businessOwner;
        }

        private bool BusinessOwnerExists(int id)
        {
            if (businessOwnerBL.GetBusinessOwner(id) != null)
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
