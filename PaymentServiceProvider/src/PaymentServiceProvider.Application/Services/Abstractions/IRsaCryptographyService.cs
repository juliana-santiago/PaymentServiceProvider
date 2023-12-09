namespace PaymentServiceProvider.Application.Services.Abstractions
{
    public interface IRsaCryptographyService
    {
        string Encrypt(string dataToEncrypt);
        string Decrypt(string dataToDecrypt);
    }
}
