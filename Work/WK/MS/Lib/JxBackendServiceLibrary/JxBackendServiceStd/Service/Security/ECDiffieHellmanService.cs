using JxBackendService.Model.Security;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using JxBackendService.Interface.Service.Security;

namespace JxBackendService.Service.Security
{
    public class ECDiffieHellmanService : IECDiffieHellmanService
    {
        private static readonly X9ECParameters s_curveECParams = ECNamedCurveTable.GetByName("secp256r1");

        private static readonly ECDomainParameters s_ecDomainParams = new ECDomainParameters(
            s_curveECParams.Curve,
            s_curveECParams.G,
            s_curveECParams.N,
            s_curveECParams.H,
            s_curveECParams.GetSeed());

        public Coordinate CalculatePublicKey(BigInteger privateKey)
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

        public BigInteger CalculateRoomKey(BigInteger userPrivateKey, Coordinate roomPublicKey)
        {
            ECPoint publicKeyPoint = s_curveECParams.Curve.CreatePoint(roomPublicKey.ToXBigInt(), roomPublicKey.ToYBigInt());
            // 创建公钥参数对象
            var publicKeyParams = new ECPublicKeyParameters(publicKeyPoint, s_ecDomainParams);
            var privateKeyParams = new ECPrivateKeyParameters(userPrivateKey, s_ecDomainParams);

            IBasicAgreement agreement = AgreementUtilities.GetBasicAgreement("ECDH");
            agreement.Init(privateKeyParams);
            BigInteger roomKey = agreement.CalculateAgreement(publicKeyParams);

            return roomKey;
        }

        public BigInteger GeneratePrivateKeySeed()
        {
            var secureRandom = new SecureRandom();
            var ecKeyGenerationParameters = new ECKeyGenerationParameters(s_ecDomainParams, secureRandom);
            var generator = new ECKeyPairGenerator("ECDH");
            generator.Init(ecKeyGenerationParameters);
            AsymmetricCipherKeyPair keyPair = generator.GenerateKeyPair();

            ECPrivateKeyParameters ecPrivateKeyParams = (ECPrivateKeyParameters)keyPair.Private;

            return ecPrivateKeyParams.D;
        }
    }
}