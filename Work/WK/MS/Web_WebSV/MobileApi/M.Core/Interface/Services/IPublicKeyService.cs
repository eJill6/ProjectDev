using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.Security;
using Org.BouncyCastle.Math;

namespace M.Core.Interface.Services
{
    public interface IPublicKeyService
    {
        Coordinate GetPublicKeyInfo();

        BaseReturnDataModel<BigInteger> GetRoomKey(string coordinate);
    }
}