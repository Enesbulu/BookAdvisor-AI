using BookAdvisor.Application.DTOs.Auth;
using BookAdvisor.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookAdvisor.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public IdentityService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }


        public async Task<(string Token, bool Success, string ErrorMessage, string UserId)> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return (Token: string.Empty, Success: false, ErrorMessage: "User not found.", UserId: string.Empty);
            }
            var checkPassword = await _userManager.CheckPasswordAsync(user: user, password: request.Password);
            if (!checkPassword)
                return (Token: string.Empty, Success: false, ErrorMessage: "Hatalı Şifre", UserId: string.Empty);
            //Token Oluşturma İşlemi
            var token = GenerateJwtToken(user: user)!;

            return (Token: token, Success: true, ErrorMessage: string.Empty, UserId: user.Id);
        }

        public async Task<(bool Success, string UserId, string ErrorMessage)> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return (Success: false, UserId: string.Empty, ErrorMessage: "Email already in use.");
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UseSystemKeys = true // Varsayılan olarak sistem key'ini kullanır
            };
            var result = await _userManager.CreateAsync(user: user, password: request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                return (Success: false, UserId: string.Empty, ErrorMessage: errors);
            }

            return (Success: true, UserId: user.Id, ErrorMessage: string.Empty);

        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(3),   //token 3 gün geçerli
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptior);

            return tokenHandler.WriteToken(token);
        }

    }
}
