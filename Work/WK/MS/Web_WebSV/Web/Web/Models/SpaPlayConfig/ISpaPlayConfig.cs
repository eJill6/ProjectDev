using System;
using System.Collections.Generic;

namespace Web.Models.SpaPlayConfig
{
    /// <summary>
    /// Spa Play Config interface
    /// </summary>
    public interface ISpaPlayConfig
    {
        /// <summary>
        /// 玩法大類
        /// </summary>
        int GameTypeId { get; }

        /// <summary>
        /// 所有的玩法大類
        /// </summary>
        /// <param name="closeDateTime">封單時間</param>
        /// <returns>玩法陣列</returns>
        IList<PlayMode<PlayTypeInfo>> Get(DateTime? closeDateTime);
    }
}