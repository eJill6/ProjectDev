using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Security;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.Security;
using M.Core.Interface.Services;
using Org.BouncyCastle.Math;

namespace M.Core.Services
{
    public class PublicKeyService : IPublicKeyService
    {
        private static Coordinate s_mobilePublicKeyInfo;

        private static string s_mobilePrivateKey;

        private readonly Lazy<IECDiffieHellmanService> _ecDiffieHellmanService;

        private readonly Lazy<IMobileApiAppSettingService> _mobileApiAppSettingService;

        public PublicKeyService()
        {
            _ecDiffieHellmanService = DependencyUtil.ResolveService<IECDiffieHellmanService>();
            _mobileApiAppSettingService = DependencyUtil.ResolveService<IMobileApiAppSettingService>();
        }

        public Coordinate GetPublicKeyInfo()
        {
            s_mobilePublicKeyInfo = AssignValueOnceUtil.GetAssignValueOnce(s_mobilePublicKeyInfo, () =>
            {
                string privateKey = GetMobilePrivateKey();

                return _ecDiffieHellmanService.Value.CalculatePublicKey(new BigInteger(privateKey));
            });

            return s_mobilePublicKeyInfo;
        }

        public BaseReturnDataModel<BigInteger> GetRoomKey(string coordinate)
        {
            string[] contents = coordinate.Split(',');

            if (!contents.Any() || contents.Length != 2)
            {
                return new BaseReturnDataModel<BigInteger>(ReturnCode.ParameterIsInvalid);
            }

            var userPublicKey = new Coordinate
            {
                X = contents[0],
                Y = contents[1]
            };

            string mobilePrivateKey = GetMobilePrivateKey();

            BigInteger userRoomKey = _ecDiffieHellmanService.Value.CalculateRoomKey(new BigInteger(mobilePrivateKey), userPublicKey);

            return new BaseReturnDataModel<BigInteger>(ReturnCode.Success, userRoomKey);
        }

        private string GetMobilePrivateKey()
        {
            throw new NotImplementedException();
        }
    }
}