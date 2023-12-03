using Microsoft.AspNetCore.Mvc;
using Work.Controllers.Models;

namespace Work.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {

        public IActionResult Get(Guid id)
        {
            throw new NotImplementedException("TODO");
        }

        public IActionResult Post(UserDto user)
        {
            throw new NotImplementedException("TODO");
        }
        
        public IActionResult Put(UserDto user)
        {
            throw new NotImplementedException("TODO");
        }

        public IActionResult Delete(Guid id)
        {
            throw new NotImplementedException("TODO");
        }

    }
}