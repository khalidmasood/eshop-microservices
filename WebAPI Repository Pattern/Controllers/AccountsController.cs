using Microsoft.AspNetCore.Mvc;
using WebAPI.Repository;
using WebAPI.Repository.Data;

namespace WebAPI_Repository_Pattern.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {


        private readonly IRepository<Account> _accountRepo;

        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IRepository<Account> repository, ILogger<AccountsController> logger)
        {
            _accountRepo = repository;
            _logger = logger;
        }

        [HttpGet("id:guid")]
        public ActionResult<Account> GetById(Guid id)
        {
            var account = _accountRepo.GetByIdAsync(id);

            if (account == null) {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public ActionResult<Account> Create([FromBody]Account account)
        {
            _accountRepo.AddAsync(account);

            return CreatedAtAction(nameof(GetById), new { id = account.Id}, account);

        }

        [HttpPut("id:guid")]
        public ActionResult<Account> Put(Guid id, [FromBody] Account account)
        {
            if (id != account.Id) {

                return BadRequest("Mismatched Account ID.");
            }

            var existing = _accountRepo.UpdateAsync(account);

            if (existing == null)
            {
                return NotFound();
            }

            _accountRepo.UpdateAsync(account);

            return NoContent();

        }

        [HttpDelete("id:guid")]
        public ActionResult<Account> Delete(Guid id)
        {
            var existing = _accountRepo.GetByIdAsync(id);

            if (existing == null)
            {
                return NotFound();
            }

            _accountRepo.DeleteAsync(id);

            return NoContent();

        }





    }
}
