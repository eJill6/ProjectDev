using JxBackendService.Model.ThirdParty.OB.OBEB;
using System.Collections.Generic;

namespace JxBackendService.Interface.Service.ThirdPartyTransfer.OBEB
{
    public interface ITPGameOBEBApiService
    {
        OBEBBaseResponseWtihDataModel<List<OBEBAnchor>> GetAnchorsResult();
    }
}