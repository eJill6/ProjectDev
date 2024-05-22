using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgDataBase.Model
{
    public abstract class BaseAGInfoModel
    {
        public abstract bool IsIgnoreAddProfitLoss { get; }

        public string platformType { get; set; }    //平台类型

        public string PlatformTypeName
        {
            get
            {
                if (AGConstParams.PlatformTypes.ContainsKey(platformType))
                {
                    return AGConstParams.PlatformTypes[platformType];
                }

                return null;
            }
        }

        
    }
}
