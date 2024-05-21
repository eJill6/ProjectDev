using JxBackendService.Interface.Model.MiseLive.Request;
using JxBackendService.Model.MiseLive.Response;

namespace JxBackendService.Interface.Service.Security
{
    public interface IPlatformAESService
    {
        byte[] Decrypt(byte[] data);
        string DecryptFromBase64String(string data);
        byte[] Encrypt(byte[] data);
        string EncryptToBase64String(string data);
    }
}