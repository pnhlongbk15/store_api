using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Store.Models;
using Store.Repositories;
using Store.Services;
using System.Text;

namespace Store
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            //Swagger 
            services.AddSwaggerGen();
            // Controller
            services.AddControllers();
            //services.AddControllersWithViews();

            //Cors
            services.AddCors(configs =>
            {
                configs.AddPolicy(
                    "AllowOrigin",
                    options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                );
            });

            //  Database
            services.AddDbContext<MyStoreContext>(optionsBuilder =>
            {
                var connectString = _configuration.GetConnectionString("MyStore");
                optionsBuilder.UseMySQL(connectString);
            });


            // Json Serialize
            services
                .AddControllersWithViews()
                .AddNewtonsoftJson(
                    options =>
                        options.SerializerSettings.ReferenceLoopHandling =
                            ReferenceLoopHandling.Ignore
                )
                .AddNewtonsoftJson(
                    options =>
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                );
            // Identity
            services
                .AddIdentity<UserModel, IdentityRole>()
                .AddEntityFrameworkStores<MyStoreContext>()
                .AddDefaultTokenProviders();



            // .AddTokenProvider<EmailTokenProvider<UserModel>>("emailconfirmation");
            //Email Token life time
            //services.Configure<EmailT>(opt =>
            // opt.TokenLifespan = TimeSpan.FromDays(3));

            // Truy cập IdentityOptions
            services.Configure<IdentityOptions>(options =>
            {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
                //
                //
                //options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";

            });

            // Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new
                    TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:ValidAudience"],
                    ValidIssuer = _configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])
                        )
                };
            });

            // Mail
            var mailSetting = _configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailSetting);
            services.AddSingleton<IEmailSender, MailService>();

            // Middleware
            // services.AddTransient<CheckUserMiddleware, CheckUserMiddleware>();

            // Repository
            services.AddScoped<IAccountRepository, AccountRepository>();



            services.ConfigureApplicationCookie(options =>
            {
                options.LogoutPath = null;
                options.LoginPath = "/api/product";
                options.AccessDeniedPath = "/api/address/add";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Cors
            app.UseCors(configs => configs.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // Route
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //app.UseCheckUser();

        }
    }
}
