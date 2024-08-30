using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RSAKeyController : ControllerBase
{
    private readonly RSAKeys _rsaKeys;

    public RSAKeyController(RSAKeys rsaKeys)
    {
        _rsaKeys = rsaKeys;
    }

    [HttpGet("public-key")]
    public IActionResult GetPublicKey()
    {
        return Ok(new { PublicKey = _rsaKeys.PublicKey });
    }
}