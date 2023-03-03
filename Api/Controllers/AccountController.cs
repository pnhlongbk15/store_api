using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IEmailSender _mailService;
        private readonly UserManager<UserModel> _userManager;
        public AccountController(IAccountRepository repo, IEmailSender mailService, UserManager<UserModel> userManager)
        {
            _accountRepo = repo;
            _mailService = mailService;
            _userManager = userManager;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {

            UserModel user = await _accountRepo.SignUpAsync(model);


            if (user.Id != null)
            {
                // generation of the email token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmLink = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token }, Request.Scheme);
                //var callback_url = Request.Scheme + "://" + Request.Host + confirmLink;

                var email_body = "Please click here: <a href=\"#URL#\">Click here.</a>";

                var body = email_body.Replace("#URL#", confirmLink);

                await _mailService.SendEmailAsync(user.Email, "auth", body);
                return Ok(user);
            }

            return Unauthorized();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            var isConfirmEmail = (await _userManager.FindByNameAsync(model.UserName)).EmailConfirmed;

            if (isConfirmEmail)
            {
                var result = await _accountRepo.SignInAsync(model);

                if (string.IsNullOrEmpty(result))
                {
                    return Unauthorized();
                }
                return Ok(result);

            }
            else
            {
                return BadRequest("Please confirm email.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest("Invalid email confirmation url.");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Invalid email parameter.");
            }

            //token = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            var status = result.Succeeded ? "Thank you for confirming your mail" :
                                            "Your email is not confirmed, please try again later.";

            return Ok(status);
        }
    }
}
