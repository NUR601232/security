using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Application.UseCase.Command.Security.Login;
using Application.Utils;
using Infrastructure.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Command.Security.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<LoginHandler> _logger;
        public LoginHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, JwtOptions jwtOptions, ILogger<LoginHandler> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtOptions = jwtOptions;
            _logger = logger;  
        }
        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            _logger.LogInformation("{username} is trying to login", request.UserName);
            if (user == null)
            {
                _logger.LogWarning("UserName {username} is not registered", request.UserName);
                user = await _userManager.FindByEmailAsync(request.UserName);
                if (user == null)
                {
                    _logger.LogWarning("Email {email} is not registered", request.UserName);
                    return new Result<string>(false, "User not found");
                }
            }

            if (!user.Active)
            {
                _logger.LogWarning("{username} is not active", request.UserName);
                return new Result<string>(false, "User is not active");
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
            if (signInResult.Succeeded)
            {
                _logger.LogInformation("{username} has logged in", request.UserName);
                var jwt = await GenerateJwt(user);
                return new Result<string>(jwt, true, "User not found");
            }
            return new Result<string>(false, "User not found");
        }

        private async Task<string> GenerateJwt(ApplicationUser user)
        {
            _logger.LogInformation($"Generating JWT for user {user.UserName}");
            var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

            var claims = await _userManager.GetClaimsAsync(user);
            foreach (var item in claims)
            {
                authClaims.Add(item);
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRoleName in userRoles)
            {
                var userRole = await _roleManager.FindByNameAsync(userRoleName);
                var listOfClaims = await _roleManager.GetClaimsAsync(userRole);

                foreach (var item in listOfClaims)
                {
                    authClaims.Add(item);
                }
            }

            authClaims.Add(new Claim("FullName", user.FullName));
            authClaims.Add(new Claim("UserName", user.UserName));
            authClaims.Add(new Claim("IsStaff", user.Staff.ToString()));
            authClaims.Add(new Claim("UserId", user.Id.ToString()));


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var lifeTime = _jwtOptions.ValidateLifetime ? _jwtOptions.Lifetime : 60 * 24 * 365;

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.ValidateIssuer ? _jwtOptions.ValidIssuer : null,
                audience: _jwtOptions.ValidateAudience ? _jwtOptions.ValidAudience : null,
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(lifeTime),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
