using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MobilApp_Szakdolgozat.Services
{
    //https://code-maze.com/how-to-decode-jwt-tokens-in-net/
    public static class JWTTokenService
    {
        public static async Task<string?> GetAccessToken() 
        {
            TokenResponse tokenResponse;
            using (var client = new HttpClient()) 
            { 
                var discoveryDocument = await client.GetDiscoveryDocumentAsync();
                if (discoveryDocument.IsError)
                {
                    throw new Exception(discoveryDocument.Error);
                }

                tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                { 
                    Address = discoveryDocument.TokenEndpoint, ClientId = "m2m",ClientSecret = "secret",
                    Scope = "api"
                });

                if (tokenResponse.IsError) 
                {
                    throw new Exception(tokenResponse.Error);
                }

                return tokenResponse.AccessToken;
            }
        }

        public static JwtSecurityToken ConvertJwtStringToJwtSecurityToken(string? jwt) 
        { 
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            return token;
        }

        public static List<string> DecodeJwt(JwtSecurityToken token)
        {
            var keyId = token.Header.Kid;
            var audience = token.Audiences.ToList();            
            var claims = token.Claims.Select(claim => claim.Value).ToList();
            List<string> claimList = new List<string>();

            for (int i = 0; i <=2; i++)
            {
                claimList.Add(claims[i]);
            }
                
            
            return claimList;
        }
    }
}
