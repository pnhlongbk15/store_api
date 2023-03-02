using Microsoft.AspNetCore.Identity;
using Store.Models;

namespace Store.Middleware
{
    public class CheckUserMiddleware : IMiddleware
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;

        public CheckUserMiddleware(SignInManager<UserModel> signInManager, UserManager<UserModel> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("hi");
            if (_signInManager.IsSignedIn(context.User))
            {
                Console.WriteLine("context");
                await next(context);
                return;
            }
            Console.WriteLine("out");
            await context.Response.WriteAsJsonAsync(new
            {
                status = "fail",
                message = "Please login before."
            });
        }
    }

    public static class CheckUserMiddlewareExtensions
    {
        public static void UseCheckUser(
            this IApplicationBuilder builder)
        {
            builder.UseMiddleware<CheckUserMiddleware>();
        }
    }
}
