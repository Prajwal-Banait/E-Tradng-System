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
    public class shareWebAPIController : ControllerBase
    {
        private readonly ShareBL shareBL = new ShareBL();

        public shareWebAPIController()
        {

        }

        // GET: api/ShareWebAPI
        [HttpGet]
        public ActionResult<IEnumerable<Share>> GetShares()
        {
          
            return new ActionResult<IEnumerable<Share>>(shareBL.GetShares());
        }

        // GET: api/ShareWebAPI/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Customer,BusinessOwner,Admin")]
        public ActionResult<Share> GetShare(int id)
        {
            var share = shareBL.GetShare(id);

            if (share == null)
            {
                return NotFound();
            }

            return share;
        }

        // PUT: api/ShareWebAPI/5
        
        [HttpPut("{id}")]
        [Authorize(Roles = "BusinessOwner")]
        public IActionResult PutShare(int id, Share share)
        {
            if (id != share.ShareId)
            {
                return BadRequest();
            }

            try
            {
                shareBL.UpdateShare(share);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShareExists(id))
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

        // POST: api/ShareWebAPI
        
        [HttpPost]
        [Authorize(Roles = "BusinessOwner")]
        public async Task<ActionResult<Share>> PostShare(Share share)
        {
           
            try
            {
                shareBL.CreateShare(share);
            }
            catch (DbUpdateException)
            {
                if (ShareExists(share.ShareId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetShare", new { id = share.ShareId }, share);
        }

        // DELETE: api/ShareWebAPI/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "BusinessOwner,Admin")]
        public ActionResult<Share> DeleteShare(int id)
        {
            
            var share = shareBL.GetShare(id);
            if (share == null)
            {
                return NotFound();
            }

            shareBL.DeleteShare(share.ShareId);

            
            return share;
        }

        private bool ShareExists(int id)
        {
            if (shareBL.GetShare(id) != null)
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
