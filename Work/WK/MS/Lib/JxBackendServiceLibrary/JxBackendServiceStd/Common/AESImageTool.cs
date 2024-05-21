using System;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Security;

namespace JxBackendService.Common
{
    /// <summary>
    /// 產品專用圖片加密
    /// </summary>
    public class AESImageTool
    {
        private readonly Lazy<IPlatformAESService> _platformAESService;

        public AESImageTool()
        {
            _platformAESService = DependencyUtil.ResolveService<IPlatformAESService>();
        }

        /// <summary>
        /// 針對內容去做AES加密
        /// </summary>
        /// <param name="bytes">原始內容</param>
        /// <returns>加密後的內容</returns>
        public byte[] AesEncryptionBytes(byte[] bytes) => _platformAESService.Value.Encrypt(bytes);
    }
}