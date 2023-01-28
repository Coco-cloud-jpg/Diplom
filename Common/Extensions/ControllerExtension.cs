using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Common.Extensions
{
    public static class ControllerExtension
    {
        public static string GetClaim(this ControllerBase controllerBase, string claimName)
        {
            var token = new JwtSecurityToken(controllerBase.HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1]);

            if (token == null)
                throw new Exception("Cannot find claim provided");

            return token.Claims.FirstOrDefault(item => item.Type ==  claimName)?.Value ?? throw new Exception("Cannot find claim provided");
        }
    }
}
