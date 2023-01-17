using System.Text;
using System.Security.Cryptography;
using Identity.Services.Interfaces;

namespace Identity.Services
{
    public class CryptoService: ICryptoService
    {
        private readonly SHA256 _shaProvider;
        public CryptoService()
        {
            _shaProvider = SHA256.Create();
        }
        public string ComputeSHA256(string message) => String.Join("", _shaProvider.ComputeHash(Encoding.UTF8.GetBytes(message)).Select(item => item.ToString("x2")));
    }
}
