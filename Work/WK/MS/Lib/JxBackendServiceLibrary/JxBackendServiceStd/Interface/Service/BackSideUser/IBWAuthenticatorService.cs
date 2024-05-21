using JxBackendService.Model.Param.Authenticator;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.Authenticator;

namespace JxBackendService.Interface.Service.BackSideUser
{
    public interface IBWAuthenticatorService
    {
        BaseReturnDataModel<QrCodeViewModel> GetQrCode(CreateQrCodeViewModelParam createQrCodeViewModelParam);

        BaseReturnDataModel<BWUserAuthenticatorInfo> GetUserAuthenticatorInfo(int userId);

        BaseReturnModel IsVerifyCodeValid(ValidVerifyCodeParam validVerifyCodeParam);

        BaseReturnModel CheckVerificationExpiring(int userId);
    }
}