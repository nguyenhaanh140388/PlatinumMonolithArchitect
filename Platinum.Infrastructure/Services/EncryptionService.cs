
using Microsoft.Extensions.Options;
using Platinum.Core.Abstractions.Services;
using Platinum.Core.Utils;
using System;
using static Platinum.Core.Utils.EncryptUtils;

namespace Platinum.Infrastructure.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly KeyInfo _keyInfo = new KeyInfo();

        public EncryptionService(IOptions<KeyInfo> keyInfo = null)
        {
            _keyInfo = keyInfo.Value;
        }

        public string Encrypt(string input)
        {
            var enc = EncryptUtils.EncryptStringToBytes_Aes(input, _keyInfo.Key, _keyInfo.Iv);
            return Convert.ToBase64String(enc);
        }

        public string Decrypt(string cipherText)
        {
            var cipherBytes = Convert.FromBase64String(cipherText);
            return EncryptUtils.DecryptStringFromBytes_Aes(cipherBytes, _keyInfo.Key, _keyInfo.Iv);
        }

        // EncryptStringToBytes_Aes and DecryptStringFromBytes_Aes above ...
    }
}
