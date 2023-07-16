using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Interface.Service.Finance
{
	public interface IRechargeService
	{
		BaseReturnModel RechargeAllFromMiseLive(string productCode);

		void RecheckOrdersFromMiseLive();
	}
}