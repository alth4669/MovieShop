using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        public AccountController(IAccountService accountService, IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register( [FromBody]UserRegisterModel model)
        {
            if(await _accountService.CreateUser(model))
            {
                return Ok(true);
            }
            return BadRequest(new {errorMessage = "Unable to register new user"});
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login( [FromBody]UserLoginModel model)
        {
            var user = await _accountService.ValidateUser(model);
            if (user != null)
            {
                // create JWT
                var jwt = CreateJWT(user);
                return Ok(new {token = jwt });
            }
            throw new UnauthorizedAccessException("Invalid email and password combination");
        }

        [HttpGet]
        [Route("CheckEmail/{email}")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var emailExists = await _accountService.EmailExists(email);
            return Ok(new { emailExists = emailExists });
        }

        private string CreateJWT(ActiveUserModel model)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, model.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.FamilyName, model.LastName),
                new Claim(JwtRegisteredClaimNames.GivenName, model.FirstName),
                new Claim ("language", "english"),
                new Claim ("isAdmin", (model.Id==2) ? "true" : "false")
            };

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["secretKey"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenExpiration = DateTime.UtcNow.AddHours(2);
            var tokenDetails = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Expires = tokenExpiration,
                SigningCredentials = credentials,
                Issuer = "MovieShop, Inc",
                Audience = "MovieShop Clients"
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var encodedJWT = tokenHandler.CreateToken(tokenDetails);
            return tokenHandler.WriteToken(encodedJWT);
        }
    }
}
