using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Reflection;

namespace BackSideWeb.Helpers
{
    public static class ZipHelper
    {
        private static readonly string Base64StringPrefix = "Base64:";
        public static string Decompress(string encoded)
        {
            if (IsBase64(encoded))
            {
                try
                {
                    var encodeString = encoded.Replace(Base64StringPrefix, string.Empty);
                    var byteSource = Convert.FromBase64String(encodeString);
                    var outputStream = new MemoryStream();
                    using (var compressedStream = new MemoryStream(byteSource))
                    {
                        using (var inputStream = new InflaterInputStream(compressedStream))
                        {
                            inputStream.CopyTo(outputStream);
                            outputStream.Position = 0;
                            return System.Text.Encoding.UTF8.GetString(outputStream.ToArray());
                        }
                    }
                }
                catch (Exception ex)
                {
                    return encoded;
                }
            }
            return encoded;
        }

        public static bool IsBase64(string encoded)
        {
            if (string.IsNullOrEmpty(encoded))
            {
                return false;
            }

            if (encoded.StartsWith(Base64StringPrefix))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
