using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using Microsoft.Extensions.Logging;

namespace PolyDataBase.Services
{
    /// <summary>
    /// impl ZipService
    /// </summary>
    public class ZipService : IZipService
    {
        private readonly int _zipCount = 100;

        /// <summary>
        /// prefix string of encoded data
        /// </summary>
        private static readonly string Base64StringPrefix = "Base64:";

        /// <inheritdoc cref="ILogger{ZipService}"/>
        private readonly ILogger<ZipService> _logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipService"/> class.
        /// </summary>
        /// <param name="logger">log</param>
        public ZipService(ILogger<ZipService> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc cref="IZipService.Compress(string)"/>
        public string Compress(string raw)
        {
            if (raw.Length > _zipCount)
            {

                var method = MethodBase.GetCurrentMethod();
                try
                {
                    var byteSource = System.Text.Encoding.UTF8.GetBytes(raw);
                    using (var memory = new MemoryStream())
                    {
                        using (var stream = new DeflaterOutputStream(memory))
                        {
                            stream.Write(byteSource, 0, byteSource.Length);
                            stream.Close();
                        }

                        return string.Concat(Base64StringPrefix, Convert.ToBase64String(memory.ToArray()));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{method.Name} failed, input:{raw}");
                }
            }

            return raw;
        }

        /// <inheritdoc cref="IZipService.Decompress(string)"/>
        public string Decompress(string encoded)
        {
            if (IsBase64(encoded))
            {
                var method = MethodBase.GetCurrentMethod();
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
                    _logger.LogError(ex, $"{method.Name} failed, input:{encoded}");
                }
            }

            return encoded;
        }

        /// <inheritdoc cref="IZipService.IsBase64(string)"/>
        public bool IsBase64(string encoded)
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