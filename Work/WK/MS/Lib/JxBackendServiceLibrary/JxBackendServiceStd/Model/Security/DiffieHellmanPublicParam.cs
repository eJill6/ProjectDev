using Org.BouncyCastle.Math;

namespace JxBackendService.Model.Security
{
    public class DiffieHellmanPublicParam
    {
        public string Prime { get; set; }

        public string Generator { get; set; }

        public BigInteger ToPrimeBigInt() => new BigInteger(Prime);

        public BigInteger ToGeneratorBigInt() => new BigInteger(Generator);
    }

    public class Coordinate
    {
        public string X { get; set; }

        public string Y { get; set; }

        public BigInteger ToXBigInt() => new BigInteger(X);

        public BigInteger ToYBigInt() => new BigInteger(Y);
    }
}