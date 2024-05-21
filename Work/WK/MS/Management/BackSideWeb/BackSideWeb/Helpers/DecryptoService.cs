using System.Security.Cryptography;
using System.Text;

namespace BackSideWeb.Helpers
{
    public interface IDecryptoService
    {
        string DecryptoFile(string base64String);

        bool DoNeedDecrypto(string urlString);

        Task<string> FetchSingleDownload(string imageUrl);

        Task<string> GetBase64Image(string imageUrl);
    }

    public class DecryptoService : IDecryptoService
    {
        private readonly string decryptoKey = "94a4b778g01ca4ab";

        // FetchSingleDownload method
        public async Task<string> FetchSingleDownload(string imageUrl)
        {
            var result = await GetBase64Image(imageUrl);
            return result;
        }

        // GetBase64Image method
        public async Task<string> GetBase64Image(string imageUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(imageUrl);
                var imageBytes = await response.Content.ReadAsByteArrayAsync();

                var hasDecrypto = DoNeedDecrypto(imageUrl);
                var base64String = Convert.ToBase64String(imageBytes);

                var result = hasDecrypto ? DecryptoFile(base64String) : base64String;
                return result;
            }
        }

        // DecryptoFile method
        public string DecryptoFile(string base64String)
        {
            // split the sha256 hash byte array into key and iv
            var keyPart = Encoding.UTF8.GetBytes(decryptoKey);

            var parts = base64String.Split(";base64,");
            var arrayData = Convert.FromBase64String(parts[0]);

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = keyPart;
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Padding = PaddingMode.None;

                using (var decryptor = aesAlg.CreateDecryptor())
                {
                    var resultData = decryptor.TransformFinalBlock(arrayData, 0, arrayData.Length);
                    var resultImage = $"data:image/jpeg;base64,{Convert.ToBase64String(resultData)}";
                    return resultImage;
                }
            }
        }

        // DoNeedDecrypto method
        public bool DoNeedDecrypto(string urlString)
        {
            var parts = urlString.Split('.');
            if (parts.Length < 2) return false;
            var index = Array.IndexOf(parts, "aes");
            return index > 0;
        }
    }
}