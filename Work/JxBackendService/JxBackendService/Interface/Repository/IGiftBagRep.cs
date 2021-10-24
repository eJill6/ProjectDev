using JxBackendService.Model.Entity;
using JxBackendService.Model.Entity.ChatRoom;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Audit;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel.ChatRoom;
using SLPolyGame.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Interface.Repository
{
    public interface IGiftBagRep
    {
        /// <summary>
        /// 取得開運紅利1元紅包已領取的記錄
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="actType"></param>
        /// <returns></returns>
        List<GiftBagLog> GetGiftBagByUserOpened(List<int?> userIds, int actType);
    }
}

