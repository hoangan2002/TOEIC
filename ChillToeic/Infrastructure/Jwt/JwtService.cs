
using ChillToeic.Infrastructure.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChillToeic.Jwt
{
    public class JwtService
    {
        private IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtOptions:SigningKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
        new Claim(ClaimTypes.Name, userInfo.Username),
        new Claim(ClaimTypes.Role, userInfo.Role)
       
    };
            var token = new JwtSecurityToken(_config["JwtOptions:Issuer"],
              _config["JwtOptions:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public UserModel AuthenticateUser(string username,string password )
        {
            UserModel user = null;

            //Validate the User Credentials
            //Demo Purpose, I have Passed HardCoded User Information
            if (username == "a" && password == "b")
            {
                user = new UserModel { Username= "Jignesh Trivedi",Role = "ADMIN" };
            }
            return user;
        }
    }
}
