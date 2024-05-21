using JxBackendService.DependencyInjection;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System.Collections.Generic;

namespace UnitTest.DiffieHellman
{
    [TestClass]
    public class DiffieHellmanTest : BaseTest
    {
        private readonly IDiffieHellmanService _diffieHellmanService;

        private readonly BigInteger _alicePrivateKey = new BigInteger("8");

        private readonly BigInteger _bobPrivateKey = new BigInteger("10");

        protected override JxApplication Application => JxApplication.MobileApi;

        public DiffieHellmanTest()
        {
            _diffieHellmanService = DependencyUtil.ResolveService<IDiffieHellmanService>();
        }

        [TestMethod]
        public void CreatePublicKey()
        {
            SecureRandom random = new SecureRandom();

            // 创建一个强随机数生成器
            // 生成一个随机的素数 P
            BigInteger p = BigInteger.ProbablePrime(2048, random);

            // 生成一个随机的 G 值，满足条件 g >= 2 且 g <= p - 2
            BigInteger pMinusTwo = p.Subtract(BigInteger.Two);
            BigInteger g;
            do
            {
                g = new BigInteger(2048, random);
            } while (g.CompareTo(BigInteger.Two) < 0 || g.CompareTo(pMinusTwo) > 0);

            var dhParameters = new DHParameters(p, g);

            LogUtilService.ForcedDebug($"p={p}\n\rg={g}");
        }

        [TestMethod]
        public void GenerateKeyPair()
        {
            DHParameters dhParameters = CreateSampleDHParameter();
            DHKeyGenerationParameters keyGenParameters = new DHKeyGenerationParameters(new SecureRandom(), dhParameters);
            var keyPairGenerator = GeneratorUtilities.GetKeyPairGenerator("DH") as DHKeyPairGenerator;
            keyPairGenerator.Init(keyGenParameters);

            AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();
            string privateKey = (keyPair.Private as DHPrivateKeyParameters).X.ToString();
        }

        [TestMethod]
        public void UpdateUserInfoSeed()
        {
            List<string> userids = new List<string>()
            {
                "2023081000000001",
                "2023081000000002",
                "2023081100000009",
                "2023081100000010",
                "2023081100000011",
                "2023081100000012",
                "2023081400000013",
                "2023081500000013",
                "2023081800000015",
                "2023081800000016",
                "2023081800000017",
                "2023082100000020",
                "2023082100000021",
                "2023082100000022",
                "2023082200000023",
                "2023082200000024",
            };

            string sql = "";

            DHParameters dhParameters = CreateSampleDHParameter();
            DHKeyGenerationParameters keyGenParameters = new DHKeyGenerationParameters(new SecureRandom(), dhParameters);
            var keyPairGenerator = GeneratorUtilities.GetKeyPairGenerator("DH") as DHKeyPairGenerator;
            keyPairGenerator.Init(keyGenParameters);

            foreach (string userid in userids)
            {
                AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();
                string privateKey = (keyPair.Private as DHPrivateKeyParameters).X.ToString();

                sql += $"UPDATE [IMsg].[dbo].[UserInfo] SET Seed = '{privateKey}' WHERE UserID = '{userid}';";
            }
        }

        [TestMethod]
        public void GeneratePublicKeyByPrivateKey()
        {
            DHParameters dhParameters = CreateSampleDHParameter();

            BigInteger alicePublicKey = CalculatePublicKey(_alicePrivateKey, dhParameters);
            Assert.IsTrue(alicePublicKey.Equals(new BigInteger("28")));

            BigInteger bobPublicKey = CalculatePublicKey(_bobPrivateKey, dhParameters);
            Assert.IsTrue(bobPublicKey.Equals(new BigInteger("17")));
        }

        [TestMethod]
        public void GenerateRoomKey()
        {
            DHParameters dhParameters = CreateSampleDHParameter();

            var dhBasicAgreement = new DHBasicAgreement();
            //使用alice私鑰搭配bob公鑰
            BigInteger bobPublicKey = CalculatePublicKey(_bobPrivateKey, dhParameters);
            dhBasicAgreement.Init(new DHPrivateKeyParameters(_alicePrivateKey, dhParameters));
            BigInteger roomKey1 = dhBasicAgreement.CalculateAgreement(new DHPublicKeyParameters(bobPublicKey, dhParameters));
            Assert.IsTrue(roomKey1.Equals(new BigInteger("4")));

            //使用bob私鑰搭配alice公鑰
            BigInteger alicePublicKey = CalculatePublicKey(_alicePrivateKey, dhParameters);
            dhBasicAgreement.Init(new DHPrivateKeyParameters(_bobPrivateKey, dhParameters));
            BigInteger roomKey2 = dhBasicAgreement.CalculateAgreement(new DHPublicKeyParameters(alicePublicKey, dhParameters));
            Assert.IsTrue(roomKey2.Equals(new BigInteger("4")));
        }

        [TestMethod]
        public void DiffieHellmanServiceTest()
        {
            DHParameters dhParameters = CreateSampleDHParameter();
            BigInteger bobPublicKey = _diffieHellmanService.CalculatePublicKey(_bobPrivateKey, dhParameters);
            BigInteger alicePublicKey = _diffieHellmanService.CalculatePublicKey(_alicePrivateKey, dhParameters);
            BigInteger roomKey1 = _diffieHellmanService.CalculateRoomKey(_bobPrivateKey, alicePublicKey, dhParameters);
            BigInteger roomKey2 = _diffieHellmanService.CalculateRoomKey(_alicePrivateKey, bobPublicKey, dhParameters);
            Assert.IsTrue(roomKey1.Equals(new BigInteger("4")));
            Assert.IsTrue(roomKey1.Equals(roomKey2));
        }

        private BigInteger CalculatePublicKey(BigInteger privateKey, DHParameters dhParameters)
        {
            return dhParameters.G.ModPow(privateKey, dhParameters.P);
        }

        private DHParameters CreateSampleDHParameter()
        {
            var publicParam = new DiffieHellmanPublicParam()
            {
                Prime = "47",
                Generator = "3"
            };

            var dhParameters = new DHParameters(
                p: publicParam.ToPrimeBigInt(),
                g: publicParam.ToGeneratorBigInt(),
                q: null,
                m: publicParam.ToPrimeBigInt().BitLength - 1,
                l: 0,
                j: null,
                validation: null);

            return dhParameters;
        }
    }
}