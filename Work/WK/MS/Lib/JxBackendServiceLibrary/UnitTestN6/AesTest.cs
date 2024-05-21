using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Enums;
using System;
using System.IO;

namespace UnitTestN6
{
    [TestClass]
    public class AesTest : BaseUnitTest
    {
        private readonly Lazy<IPlatformAESService> _platformAESService;

        public AesTest()
        {
            _platformAESService = DependencyUtil.ResolveService<IPlatformAESService>();
        }

        [TestMethod]
        public void EncryptImage()
        {
            //var fileInfo = new FileInfo("c:\\temp\\OIG.pDCDyVAi9.jpg");
            var fileInfo = new FileInfo("c:\\temp\\49c214f14b5c457abed0d867c4d29992.png");

            byte[] fileContent = File.ReadAllBytes(fileInfo.FullName);
            byte[] encryptContent = _platformAESService.Value.Encrypt(fileContent);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            string newFullFilePath = Path.Combine(fileInfo.DirectoryName, $"{fileNameWithoutExtension}.aes");
            File.WriteAllBytes(newFullFilePath, encryptContent);
        }

        [TestMethod]
        public void DecryptImage()
        {
            //var fileInfo = new FileInfo("c:\\temp\\OIG.pDCDyVAi9.aes"); //
            var fileInfo = new FileInfo("c:\\temp\\49c214f14b5c457abed0d867c4d29992.aes");

            byte[] fileContent = File.ReadAllBytes(fileInfo.FullName);
            byte[] decryptContent = _platformAESService.Value.Decrypt(fileContent);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.FullName);
            string newFullFilePath = Path.Combine(fileInfo.DirectoryName, $"{fileNameWithoutExtension}.png");
            File.WriteAllBytes(newFullFilePath, decryptContent);
        }

        [TestMethod]
        public void EncryptString()
        {
            string result = _platformAESService.Value.EncryptToBase64String("1234567890abcd!QAZ@!WSX");

            Assert.IsTrue(result.Equals("JYaAaqTzpVziGM3+DLX0hY3DSweQAfxi3tduEe5P67I="));
        }

        [TestMethod]
        public void DecryptString()
        {
            string result = _platformAESService.Value.DecryptFromBase64String("JYaAaqTzpVziGM3+DLX0hY3DSweQAfxi3tduEe5P67I=");
            Assert.IsTrue(result.Equals("1234567890abcd!QAZ@!WSX"));
        }
    }
}