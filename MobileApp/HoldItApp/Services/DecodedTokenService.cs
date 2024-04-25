using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilApp_Szakdolgozat.Services
{
    public record DecodedTokenService(
        string KeyId,
        string Issuer,
        List<string> Audience,
        List<(string Type, string Value)> Claims,
        DateTime Expiration,
        string SigningAlgorithm,
        string RawData,
        string Subject,
        DateTime ValidFrom,
        string Header,
        string Payload);    
}
