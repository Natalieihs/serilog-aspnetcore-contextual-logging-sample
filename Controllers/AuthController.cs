using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class AuthController : ControllerBase
{

    //注入log
    private readonly ILogger<AuthController> _logger;

    private readonly IConfiguration _config;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
        
    }
   

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginViewModel model)
    {
        // Check user credentials
        if (model.Username != "user" || model.Password != "pass")
        {
            return Unauthorized();
        }

        // Generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("This is my custom Secret key for authnetication");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "user"),
                new Claim(ClaimTypes.Role, "admin")
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new { token = tokenHandler.WriteToken(token) });
    }

    //写一个授权过的测试action
    [Authorize]
    [HttpGet("test")]
    public IActionResult Test()
    {
        _logger.LogInformation("test");
        return Ok("test");
    }


public class LoginViewModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

}
