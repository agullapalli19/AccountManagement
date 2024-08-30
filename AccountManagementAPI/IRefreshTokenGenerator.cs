using AccountManagementAPI.Model;
using System.Security.Claims;

namespace AccountManagementAPI
{
    public interface IRefreshTokenGenerator
    {
        string GenerateToken(string? username);

        TokenResponse Authenticate(string? username, Claim[] claims);
    }
}
