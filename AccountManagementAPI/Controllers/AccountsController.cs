using AccountManagementAPI.Model;
using AccountManagementAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace AccountManagementAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly RSAKeys _rsaKeys;
        private readonly IAccountsRepo _accounts;

        public AccountsController(RSAKeys rsaKeys, IAccountsRepo accounts)
        {
            _rsaKeys = rsaKeys;
            _accounts = accounts;
        }
        //[Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_accounts.GetAccounts());
        }
        //[Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_accounts.GetAccountByID(id));
        }
        //[Authorize]
        [HttpPost("Transfer")]
        public IActionResult Transfer([FromBody] TransferRequestEncrypt request)
        {
            try
            {
                var jsonString = RSADecryption.DecryptAmountTransfer(request.EncryptedData, _rsaKeys.PrivateKey);
                TransferRequest? transferRequest = System.Text.Json.JsonSerializer.Deserialize<TransferRequest>(jsonString);
                Account? act = _accounts.Transfer(transferRequest);
                if (act != null)
                {
                    return Ok(act);
                }
                else
                {
                    return Ok(ApiResponseCode.TargetAccountNumberNotFound);
                }
            }
            catch (Exception ex)
            {             
                return BadRequest(ex.Message);
            }
     
        }
    }
}

public static class RSADecryption
{
    public static string DecryptAmountTransfer(string encryptedData, string privateKey)
    {
        using (var rsa = RSA.Create())
        {
            // Convert the Base64-encoded private key to a byte array
            byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

            // Convert the Base64-encoded encrypted data to a byte array
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

            var decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256);
            string json = Encoding.UTF8.GetString(decryptedBytes);

            // Deserialize the JSON string back into the AmountTransfer object
            return json;
        }
    }
}
