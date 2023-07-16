using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums
{
    public class EnvironmentCode : BaseStringValueModel<EnvironmentCode>
    {
        private EnvironmentCode()
        { }

        public string AccountPrefixCode { get; private set; }

        public string OrderPrefixCode { get; private set; }

        public bool IsTestingEnvironment => Value != Production;

        public static readonly EnvironmentCode Development = new EnvironmentCode()
        {
            Value = "Development",
            AccountPrefixCode = "D",
            OrderPrefixCode = "D"
        };

        public static readonly EnvironmentCode SIT = new EnvironmentCode()
        {
            Value = "SIT",
            AccountPrefixCode = "S",
            OrderPrefixCode = "S"
        };

        public static readonly EnvironmentCode UAT = new EnvironmentCode()
        {
            Value = "UAT",
            AccountPrefixCode = "U",
            OrderPrefixCode = "U"
        };

        public static readonly EnvironmentCode Production = new EnvironmentCode()
        {
            Value = "Production",
            AccountPrefixCode = string.Empty,
            OrderPrefixCode = "L"
        };
    }
}