using JxBackendService.Model.Security;
using Org.BouncyCastle.Math;

namespace JxBackendService.Interface.Service.Security
{
    public interface IBaseDiffieHellmanService
    {
        BigInteger GeneratePrivateKeySeed();
    }

    public interface IECDiffieHellmanService : IBaseDiffieHellmanService
    {
        Coordinate CalculatePublicKey(BigInteger privateKey);

        BigInteger CalculateRoomKey(BigInteger userPrivateKey, Coordinate roomPublicKey);
    }
}