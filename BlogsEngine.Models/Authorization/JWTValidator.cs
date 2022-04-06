using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsEngine.Models.Authorization
{
    public class JWTValidator
    {
        private string? secret;

        public bool IsValid
        {
            get;
        }
        public int Id { get; }
        public string? Username
        {
            get;
        }
        public string? Role
        {
            get;
        }
        public JWTValidator(HttpRequest request)
        {

            this.secret = Environment.GetEnvironmentVariable("Secret", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(this.secret))
            {
                IsValid = false;
                return;
            }

            // Check if we have a header.
            if (!request.Headers.ContainsKey("Authorization"))
            {
                IsValid = false;
                return;
            }
            string authorizationHeader = request.Headers["Authorization"];
            // Check if the value is empty.
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                IsValid = false;
                return;
            }
            // Check if we can decode the header.
            IDictionary<string, object> claims = null;
            try
            {
                if (authorizationHeader.StartsWith("Bearer"))
                {
                    authorizationHeader = authorizationHeader.Substring(7);
                }
                // Validate the token and decode the claims.
                claims = new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm()).WithSecret(this.secret).MustVerifySignature().Decode<IDictionary<string, object>>(authorizationHeader);
            }
            catch (Exception exception)
            {
                IsValid = false;
                return;
            }
            // Check if we have user claim.
            if (!claims.ContainsKey("UserName"))
            {
                IsValid = false;
                return;
            }
            IsValid = true;
            Id = Convert.ToInt32(claims["Id"]);
            Username = Convert.ToString(claims["UserName"]);
            Role = Convert.ToString(claims["Role"]);
        }
    }
}
