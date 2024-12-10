using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ONIK_BANK.DTO;
using ONIK_BANK.IService;
using ONIK_BANK.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;


using static ONIK_BANK.DTO.Responses;

namespace ONIK_BANK.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AuthService(UserManager<AppUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<GeneralResponse> CreateAccount(RegisterDTO userDTO)
        {
            if (userDTO == null)
            {
                return new GeneralResponse(false, "User data cannot be empty.");
            }

            
            var existingUser = await userManager.FindByEmailAsync(userDTO.Email);
            if (existingUser != null)
            {
                return new GeneralResponse(false, "This email already exists.");
            }

            
            var newUser = new AppUser()
            {
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                PhoneNumber = userDTO.PhoneNumber,
                UserName = userDTO.Email
            };

            var createUserResult = await userManager.CreateAsync(newUser, userDTO.Password);
            if (!createUserResult.Succeeded)
            {
                return new GeneralResponse(false, "An error occurred while creating the account.");
            }

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            await userManager.AddToRoleAsync(newUser, "User");

            return new GeneralResponse(true, "Account successfully created.");
        }

        public async Task<LogInResponse> LogInAccount(LogInDTO logInDTO)
        {
            if (logInDTO == null)
            {
                return new LogInResponse(false, null, "The Login Container is empty");
            }
            var getUser = await userManager.FindByEmailAsync(logInDTO.Email);
            if (getUser == null)
            {
                return new LogInResponse(false, null, "The email is Incorrect/User nor found");
            }
            bool IsPasswordChecked = await userManager.CheckPasswordAsync(getUser, logInDTO.Password);
            if (!IsPasswordChecked)
            {
                return new LogInResponse(false, null, "Invalid Email/Password");
            }
            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.FirstName, getUser.LastName, getUser.Email, getUserRole.First());
            string token = GenerateToken(userSession);
            return new LogInResponse(true, token, "Login succesful");
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim (ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Name, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                
            };

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
