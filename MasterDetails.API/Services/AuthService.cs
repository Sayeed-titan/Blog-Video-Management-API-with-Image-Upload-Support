using MasterDetails.API.DTOs;
using MasterDetails.API.Entities;
using MasterDetails.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MasterDetails.API.Services
{
    public class AuthService(IUnitOfWork unitOfWork, IConfiguration configuration) : IAuthService
    {
        public async Task<TokenResponseDto> LoginAsync(LoginDto request)
        {
            var user = await unitOfWork.Users.GetAsync(
                u => u.Email == request.Email,
                includeProperties: "Role"
            );

            if (user is null)
            {
                return null;
            }

            var passwordHasher = new PasswordHasher<User>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(
                user, user.PasswordHash, request.Password
            );

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            if (user.Role == null || string.IsNullOrWhiteSpace(user.Role.RoleName))
                throw new Exception("User does not have a role assigned!");

            return await CreateTokenResponse(user);
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = GenerateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        public async Task<User?> RegisterAsync(UserDto request)
        {

            var existingUser = await unitOfWork.Users.GetAsync(u => u.Email == request.Email);
            if (existingUser != null)
                throw new ApplicationException("A user with this email already exists.");

            var role = await unitOfWork.Roles.GetAsync(u => u.RoleID == request.RoleId);
            if (role is null) throw new Exception("Invalid Role Assigned !");

            var user = new User();

            var hashedPassword = new PasswordHasher<User>()
               .HashPassword(user, request.Password);

            user.UserID = request.UserId;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PasswordHash = hashedPassword;
            user.Email = request.Email;
            user.RoleID = request.RoleId;

            unitOfWork.Users.AddAsync(user);

            await unitOfWork.SaveAsync();

            return user;
        }

        public string GenerateToken(User user)
        {
            var jwtSettings = configuration.GetSection("jwtSettings");
            var key = jwtSettings["Key"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiresInMinutes = Convert.ToInt32(jwtSettings["AccessTokenExpirationMinutes"] ?? "200");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? string.Empty)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string?> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefeshToken = refreshToken;
            user.RefreshTokenExpiryTme = DateTime.Now.AddDays(7);

            await unitOfWork.SaveAsync();

            return refreshToken;
        }

        private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await unitOfWork.Users.GetAsync(a => a.UserID == userId, includeProperties: "Role");

            if (user is null || user.RefreshTokenExpiryTme <= DateTime.Now || user.RefeshToken != refreshToken)
            {
                return null;
            }
            return user;
        }

        public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            if (user is null)
                return null;

            return await CreateTokenResponse(user);
        }


    }
}
