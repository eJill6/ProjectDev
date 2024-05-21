using MS.Core.MM.Models.Entities.HomeAnnouncement;
using MS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.Core.MM.Services.interfaces
{
    public interface IHomeAnnouncementService
    {
        Task<BaseReturnDataModel<IEnumerable<MMHomeAnnouncement>>> Get();

        Task<BaseReturnModel> Update(MMHomeAnnouncement param);

        Task<BaseReturnModel> Create(MMHomeAnnouncement param);

        /// <summary>
        /// 刪除HomeAnnouncement資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns>刪除的結果</returns>
        Task<BaseReturnModel> Delete(int id);
    }
}