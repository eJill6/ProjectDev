using JxBackendService.Model.ServiceModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using System.Collections.Generic;
using System.ServiceModel;

namespace JxBackendService.Interface.Service
{
    [ServiceContract]
    public interface ISlotApiService
    {
        [OperationContract, CommonMOperationBehavior]
        List<GameLobbyInfo> GetGameList(string gameLobbyType);

        [OperationContract, CommonMOperationBehavior]
        string GetJackpotAmount(string gameLobbyType);
    }
}