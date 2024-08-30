using AccountManagementAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AccountManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly AccountDBContext _context;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IConfiguration _config;
        private readonly RSAKeys _rsaKeys;

        public AuthenticateController(AccountDBContext accountDBContext, IConfiguration config, IRefreshTokenGenerator refreshTokenGenerator, RSAKeys rsaKeys)
        {
            _context = accountDBContext;
            _refreshTokenGenerator = refreshTokenGenerator;
            _config = config;
            _rsaKeys = rsaKeys;
          
        }

        [HttpPost()]
        public async Task<IActionResult> Authenticate([FromBody] TransferRequestEncrypt request)
        {
            var jsonString = RSADecryption.DecryptAmountTransfer(request.EncryptedData, _rsaKeys.PrivateKey);
            usercred? user = System.Text.Json.JsonSerializer.Deserialize<usercred>(jsonString);
            TokenResponse tokenResponse = new TokenResponse();
            var _user = _context.People.FirstOrDefault(o => o.Username == user.username && o.Password == user.password);
            if (_user == null)
            {
                return Unauthorized();
            }
            var tokenhandler = new JwtSecurityTokenHandler();
            var securityKey = _config.GetValue<String>("JWTSetting:securityKey");
            var tokenkey = Encoding.UTF8.GetBytes(securityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.username),
                        new Claim(ClaimTypes.GivenName, _user.FirstName + " " + _user.LastName),
                        new Claim(ClaimTypes.Upn, _user.PersonId.ToString()),
                    }

                    ),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)

            };
            var token = tokenhandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenhandler.WriteToken(token);

            tokenResponse.JWTToken = finaltoken;
            tokenResponse.RefreshToken = _refreshTokenGenerator.GenerateToken(user.username);

            return Ok(tokenResponse);
        }

        
        [HttpPost("Refresh")]
        public IActionResult Refresh([FromBody] TokenResponse token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var securityKey = _config.GetValue<String>("JWTSetting:securityKey");
            var principal = tokenHandler.ValidateToken(token.JWTToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out securityToken);

            var _token = securityToken as JwtSecurityToken;

            if (_token != null && _token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return Unauthorized();
            }
            var username = principal.Identity != null ? principal.Identity.Name : String.Empty;
            var _reftable = _context.TblRefreshTokens.FirstOrDefault(o => o.UserId == username && o.RefreshToken == token.RefreshToken);

            if (_reftable == null)
            {
                return Unauthorized();
            }
            TokenResponse _result = _refreshTokenGenerator.Authenticate(username, principal.Claims.ToArray());
            return Ok(_result);
        }


    }
}
