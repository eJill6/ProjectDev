using System;
using System.Data;
using Dapper;
using JxBackendService.Common.Util;
using Newtonsoft.Json;

namespace JxBackendService.TypeHandler
{
    public class JsonStringHandler : SqlMapper.ITypeHandler
    {
        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.Value = value == null ? string.Empty : value.ToJsonString();
        }

        public object Parse(Type destinationType, object value)
        {
            return value == null ? null : JsonConvert.DeserializeObject(value.ToString(), destinationType);
        }
    }
}
