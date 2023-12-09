using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentServiceProvider.Application.Services.Abstractions;
using PaymentServiceProvider.Infrastructure.Dto.Configurations;
using PaymentServiceProvider.Util.Constants;
using System.Security.Cryptography;
using System.Text;

namespace PaymentServiceProvider.Application.Services
{
    public class RsaCryptographyService : IRsaCryptographyService
    {
        private readonly SecurityOptions _securityConfig;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RsaCryptographyService> _logger;
        private readonly string _logTag;

        public RsaCryptographyService(
            IOptions<SecurityOptions> options,
            ILogger<RsaCryptographyService> logger,
            IConfiguration configuration)
        {  
            _logger = logger;
            _logTag = Constants.TagRsaCryptographyService;
            _configuration = configuration;
            _securityConfig = new SecurityOptions();
            _configuration.GetSection(nameof(SecurityOptions)).Bind(_securityConfig);
        }

        public string Encrypt(string dataToEncrypt)
        {
            try
            {
                _logger.LogInformation($"{_logTag}[Encrypt] called");

                byte[] encryptedData;
                byte[] dataToEncryptBytes = _encoding.GetBytes(dataToEncrypt);

                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(_securityConfig.PublicKey);
                    encryptedData = rsa.Encrypt(dataToEncryptBytes, false);
                }

                return encryptedData.ToString();
            }
            catch (CryptographicException ex)
            {
                _logger.LogError($"{_logTag}[RSAEncrypt][Error] encrypting value - {ex.Message}");
                throw;
            }
        }

        public string Decrypt(string dataToDecrypt)
        {
            try
            {
                _logger.LogInformation($"{_logTag}[Decrypt] called");
                byte[] decryptedData;
                byte[] dataToDecryptBytes = _encoding.GetBytes(dataToDecrypt);

                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(_securityConfig.PublicKey);
                    decryptedData = rsa.Decrypt(dataToDecryptBytes, false);
                }

                return _encoding.GetString(decryptedData);
            }
            catch (CryptographicException ex)
            {
                _logger.LogError($"{_logTag}[RSADecrypt][Error] decrypting value - {ex.Message}");
                throw;
            }
        }
    }
}
