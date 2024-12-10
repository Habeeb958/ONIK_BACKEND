using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ONIK_BANK.Data;
using ONIK_BANK.IService;
using ONIK_BANK.Models;
using ONIK_BANK.Service;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace ONIK_BANK
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Configure Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Please enter 'Bearer' [space] and then your token",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });
                opt.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            // Get the connection string
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Use PostgreSQL Database
            builder.Services.AddDbContext<CustomerDbContext>(options =>
                options.UseNpgsql(connectionString));

            /* // Use MySQL Database (Commented Out)
            builder.Services.AddDbContext<CustomerDbContext>(option =>
                option.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            */

            // Register services
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Configure Identity
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
                opt.SignIn.RequireConfirmedEmail = true;
                opt.SignIn.RequireConfirmedPhoneNumber = true;
                opt.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<CustomerDbContext>()
            .AddSignInManager()
            .AddRoles<IdentityRole>();

            // Configure JWT Authentication
            builder.Services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
                };
            });

            // Configure CORS to allow requests from your frontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
