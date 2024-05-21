using JxBackendService.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class GameTabTypeValue : BaseIntValueModel<GameTabTypeValue>
    {
        public static readonly GameTabTypeValue All = new GameTabTypeValue { Value = 1, };

        public static readonly GameTabTypeValue Hot = new GameTabTypeValue { Value = 2, };

        public static readonly GameTabTypeValue Favorite = new GameTabTypeValue { Value = 3, };
    }
}