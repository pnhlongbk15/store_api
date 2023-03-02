using Microsoft.AspNetCore.Mvc;
using Store.Models;
using Store.Repositories;

namespace Store.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;
        public AccountController(IAccountRepository repo)
        {
            _accountRepo = repo;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {

            var result = await _accountRepo.SignUpAsync(signUpModel);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return Unauthorized();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            var result = await _accountRepo.SignInAsync(signInModel);
            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }
            return Ok(result);
        }
    }
}
