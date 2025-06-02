using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceImp
{
	public class JwtHelper
	{
		private readonly IConfiguration _configuration;

		public JwtHelper(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateToken(string email, int? role)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings");
			var secretKey = jwtSettings["SecretKey"];

			if (string.IsNullOrEmpty(secretKey))
				throw new Exception("SecretKey is missing in appsettings.json");

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

			var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Email, email),
			new Claim(ClaimTypes.Role, role?.ToString() ?? "0")
		};

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: jwtSettings["Issuer"],
				audience: jwtSettings["Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["AccessTokenExpirationMinutes"] ?? "60")),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}



}
