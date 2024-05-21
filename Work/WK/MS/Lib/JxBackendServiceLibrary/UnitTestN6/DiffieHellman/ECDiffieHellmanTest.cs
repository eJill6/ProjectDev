using System;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Security;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using UnitTestN6;

namespace UnitTest.DiffieHellman
{
    [TestClass]
    public class ECDiffieHellmanTest : BaseUnitTest
    {
        private readonly Lazy<IECDiffieHellmanService> _ecDiffieHellmanService;

        private readonly Lazy<ILogUtilService> _logUtilService;

        private static readonly X9ECParameters s_curveECParams = ECNamedCurveTable.GetByName("secp256r1");

        private static readonly ECDomainParameters s_ecDomainParams = new ECDomainParameters(
            s_curveECParams.Curve,
            s_curveECParams.G,
            s_curveECParams.N,
            s_curveECParams.H,
            s_curveECParams.GetSeed());

        private readonly BigInteger _alicePrivateKey = new BigInteger("8");

        private readonly BigInteger _bobPrivateKey = new BigInteger("10");

        private readonly BigInteger _mobilePrivateKey = new BigInteger("58879247872359737192647643173138716901210792759286662204075188319470732859006");

        private readonly BigInteger _userPrivateKey = new BigInteger("69252817155018045136575526284563154419266215112934712245315417324121765342209");

        public ECDiffieHellmanTest()
        {
            _ecDiffieHellmanService = DependencyUtil.ResolveService<IECDiffieHellmanService>();
            _logUtilService = DependencyUtil.ResolveService<ILogUtilService>();
        }

        [TestMethod]
        public void CreateECPoint()
        {
            // 创建一个椭圆曲线域参数
            X9ECParameters curveParams = ECNamedCurveTable.GetByName("secp256r1"); // 使用 secp256r1 曲线参数

            // 获取基点 "G"
            ECPoint generator = curveParams.G;

            _logUtilService.Value.ForcedDebug($"x={generator.XCoord.ToBigInteger()}\n\ry={generator.YCoord.ToBigInteger()}");
        }

        [TestMethod]
        public void GenerateKeyPair()
        {
            //ECDH
            var keyPairGenerator = new ECKeyPairGenerator("ECDH");
            var secureRandom = new SecureRandom();
            var ecgp = new ECKeyGenerationParameters(s_ecDomainParams, secureRandom);
            keyPairGenerator.Init(ecgp);
            AsymmetricCipherKeyPair eckp = keyPairGenerator.GenerateKeyPair();

            ECPublicKeyParameters ecPub = (ECPublicKeyParameters)eckp.Public;
            ECPrivateKeyParameters ecPri = (ECPrivateKeyParameters)eckp.Private;

            string seed = ecPri.D.ToString();
            _logUtilService.Value.ForcedDebug(seed);
        }

        [TestMethod]
        public void GenerateRoomKeyTest()
        {
            //generate bob public key
            Coordinate bobPublicKey = CalculatePublicKey(_bobPrivateKey);
            //generate alice public key
            Coordinate alicePublicKey = CalculatePublicKey(_alicePrivateKey);

            //使用alice私鑰搭配bob公鑰
            BigInteger roomKey1 = CalculateAgreement(_alicePrivateKey, bobPublicKey);

            //使用bob私鑰搭配alice公鑰
            BigInteger roomKey2 = CalculateAgreement(_bobPrivateKey, alicePublicKey);

            Assert.AreEqual(roomKey1, roomKey2);
        }

        [TestMethod]
        public void ServiceTest()
        {
            //generate bob public key
            Coordinate bobPublicKey = _ecDiffieHellmanService.Value.CalculatePublicKey(_bobPrivateKey);
            //generate alice public key
            Coordinate alicePublicKey = _ecDiffieHellmanService.Value.CalculatePublicKey(_alicePrivateKey);

            //使用alice私鑰搭配bob公鑰
            BigInteger roomKey1 = _ecDiffieHellmanService.Value.CalculateRoomKey(_alicePrivateKey, bobPublicKey);

            //使用bob私鑰搭配alice公鑰
            BigInteger roomKey2 = _ecDiffieHellmanService.Value.CalculateRoomKey(_bobPrivateKey, alicePublicKey);

            Assert.AreEqual(roomKey1, roomKey2);
        }

        [TestMethod]
        public void GetUserPublicKey()
        {
            Coordinate coordinate = _ecDiffieHellmanService.Value.CalculatePublicKey(_userPrivateKey);

            string userPublicKey = $"{coordinate.X},{coordinate.Y}";
        }

        private Coordinate CalculatePublicKey(BigInteger privateKey)
        {
            // 根据私钥生成公钥
            ECPoint publicKeyPoint = s_curveECParams.G.Multiply(privateKey); // 公钥 = 基点 "G" * 私钥
            var publicKeyParams = new ECPublicKeyParameters(publicKeyPoint, s_ecDomainParams);

            var coordinate = new Coordinate()
            {
                X = publicKeyParams.Q.AffineXCoord.ToBigInteger().ToString(),
                Y = publicKeyParams.Q.AffineYCoord.ToBigInteger().ToString(),
            };

            return coordinate;
        }

        private BigInteger CalculateAgreement(BigInteger privateKey, Coordinate publicKey)
        {
            ECPoint publicKeyPoint = s_curveECParams.Curve.CreatePoint(publicKey.ToXBigInt(), publicKey.ToYBigInt());
            // 创建公钥参数对象
            var publicKeyParams = new ECPublicKeyParameters(publicKeyPoint, s_ecDomainParams);
            var privateKeyParams = new ECPrivateKeyParameters(privateKey, s_ecDomainParams);

            IBasicAgreement agreement = AgreementUtilities.GetBasicAgreement("ECDH");
            agreement.Init(privateKeyParams);
            BigInteger sharedSecret = agreement.CalculateAgreement(publicKeyParams);

            return sharedSecret;
        }
    }
}