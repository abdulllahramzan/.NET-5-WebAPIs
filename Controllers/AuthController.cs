using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using myapp.Data;
using myapp.Dtos.User;
using myapp.Models;

namespace myapp.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;

        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register (UserRegisterDto request)
        {
            var response = await _authRepo.Register(
                new User {Username = request.Username}, request.Password
            );

            if(!response.Success)
            {
                return BadRequest(request);
            }
                return Ok(response);
        }

         [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login (UserLoginDto request)
        {
            var response = await _authRepo.Login(
               request.Username, request.Password
            );

            if(!response.Success)
            {
                return BadRequest(request);
            }
                return Ok(response);
        }

    }
}