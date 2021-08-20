using System.Linq;
using AccountService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        private readonly AccountDbContext _dbContext;

        public HomeController(AccountDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<UserHistory> GetUserHistory()
        {
            var userHistory = _dbContext.UserHistory.ToList();
            return Ok(userHistory);
        }

        [HttpGet("{id}")]
        public ActionResult<UserHistory> GetIdHistory([FromRoute] int id)
        {
            var getUser = _dbContext.User.Where(r => r.Id == id);
            if (getUser is null)
            {
                return NotFound("Not found User history");
            }

            return Ok(getUser);
        }
    }
}