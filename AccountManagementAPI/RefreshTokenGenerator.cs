using AccountManagementAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace AccountManagementAPI
{
    public class RefreshTokenGenerator: IRefreshTokenGenerator
    {
        private readonly IConfiguration _config;
        
        public RefreshTokenGenerator(IConfiguration config)
        {
            _config = config;
      
        }
        public string GenerateToken(string? username)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AccountDBContext>();
            var test = _config.GetConnectionString("AccountDB");
            optionsBuilder.UseSqlServer(_config.GetConnectionString("AccountDB"));
            AccountDBContext _context = new AccountDBContext(optionsBuilder.Options);

            var randomnumber = new byte[32];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string RefreshToken = Convert.ToBase64String(randomnumber);
                var _user = _context.TblRefreshTokens.FirstOrDefault(o => o.UserId == username);
                if (_user != null)
                {
                    _user.RefreshToken = RefreshToken;
                    _context.SaveChanges();
                }
                else
                {
                    TblRefreshToken tbl_RefreshToken = new TblRefreshToken
                    {

                        UserId = username != null ? username : String.Empty,
                        TokenId = new Random().Next().ToString(),
                        RefreshToken = RefreshToken,
                        IsActive = 1
                    };
                    // _context.tbl_RefreshToken.Add(tbl_RefreshToken);
                    //_context.SaveChanges();
                }
                return RefreshToken;
            }

        }
        
        public TokenResponse Authenticate(string? username, Claim[] claims)
        {
            TokenResponse tokenresponse = new TokenResponse();
            var securityKey = _config.GetValue<String>("JWTSetting:securityKey");
            var tokenKey = Encoding.UTF8.GetBytes(securityKey);
            var tokenhandler = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
                );
            tokenresponse.JWTToken = new JwtSecurityTokenHandler().WriteToken(tokenhandler);
            tokenresponse.RefreshToken = this.GenerateToken(username);
            return tokenresponse;

        }
    }
}
