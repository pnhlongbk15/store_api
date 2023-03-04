using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Store.Models;
using Store.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Store.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IConfiguration _configuration;
        public AccountRepository(UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<string> SignInAsync(SignInModel model)
        {
            var result = await _signInManager.PasswordSignInAsync
                        (model.UserName, model.Password, false, false);

            if (!result.Succeeded)
            {
                return string.Empty;
            }

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            //var authenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);// khong dung duoc
            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(20),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authenKey,
                                            SecurityAlgorithms.HmacSha512Signature)
                    );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<dynamic> SignUpAsync(SignUpModel model)
        {
            var user = new UserModel
            {
                UserName = model.UserName,
                Email = model.Email,
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            Console.WriteLine(result);
            if (result.Succeeded)
            {
                return user;
            }
            ErrorSignUp.Messages = null;
            ErrorSignUp.Messages = result.Errors.ToList();

            return String.Empty;
        }
    }
}
