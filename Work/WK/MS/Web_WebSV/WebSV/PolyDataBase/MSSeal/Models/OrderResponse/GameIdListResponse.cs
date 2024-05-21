using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLPolyGame.Web.MSSeal.Models.OrderResponse
{
    public class GameIdListResponse
    {
        public List<OrderTypeInfo> OrderTypeInfos { get; set; }

        public List<OrderSubTypeInfo> OrderSubTypeInfos { get; set; }

        public List<GameIdInfo> GameIdInfos { get; set; }
    }
}