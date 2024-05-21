using System.Threading.Tasks;

namespace PolyDataBase.Services
{
    /// <summary>
    /// service of zip
    /// </summary>
    public interface IZipService
    {
        /// <summary>
        /// compress
        /// </summary>
        /// <param name="raw">raw string</param>
        /// <returns>compressed</returns>
        string Compress(string raw);

        /// <summary>
        /// Decompress
        /// </summary>
        /// <param name="encoded">encoded string</param>
        /// <returns>decompressed</returns>
        string Decompress(string encoded);

        /// <summary>
        /// is base 64 string
        /// </summary>
        /// <param name="encoded">encoded string</param>
        /// <returns>true: is base 64 encoded, false: not base 64 encoded</returns>
        bool IsBase64(string encoded);
    }
}