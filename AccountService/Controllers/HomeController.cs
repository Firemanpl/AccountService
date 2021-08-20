using System.Linq;
using AccountService.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        private readonly AccountDbContext _dbcontext;

        public HomeController(AccountDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public ActionResult<UserHistory> GetUserHistory()
        {
            var userhistory = _dbcontext.UserHistory.ToList();
            return Ok(userhistory);
        }

        [HttpGet("{id}")]
        public ActionResult<UserHistory> GetIdHistory([FromRoute] int id)
        {
            var getuser = _dbcontext.User.Where(r => r.Id == id);
            if (getuser is null)
            {
                return NotFound("Not found User history");
            }

            return Ok(getuser);
        }
    }
}