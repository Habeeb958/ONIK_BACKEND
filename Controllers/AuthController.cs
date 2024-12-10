using Microsoft.AspNetCore.Mvc;
using ONIK_BANK.DTO;
using ONIK_BANK.IService;
using ONIK_BANK.Service;

namespace ONIK_BANK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost]
        [Route("RegisterUser")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDTO addUser)
        {
            var response = await _auth.CreateAccount(addUser);
            return Ok(response);
        }



        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LogIn([FromBody] LogInDTO login)
        {
            var response = await _auth.LogInAccount(login);
            return Ok(response);
        }

        
    }
}
