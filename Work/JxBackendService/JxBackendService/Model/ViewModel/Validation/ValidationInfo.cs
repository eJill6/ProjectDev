using JxBackendService.Model.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.ViewModel.Validation
{
    public class FormInputColumn : BaseStringValueModel<FormInputColumn>
    {
        private FormInputColumn() { }

        public static readonly FormInputColumn UserName = new FormInputColumn() { Value = "UserName" };

        public static readonly FormInputColumn LoginPassword = new FormInputColumn() { Value = "LoginPassword" };

        public static readonly FormInputColumn InviteCode = new FormInputColumn() { Value = "InviteCode" };

        public static readonly FormInputColumn GraphicValidateCode = new FormInputColumn() { Value = "GraphicValidateCode" };

        public static readonly FormInputColumn Email = new FormInputColumn() { Value = "Email" };

        public static readonly FormInputColumn Phone = new FormInputColumn() { Value = "Phone" };
    }

    public class ValidationInfo
    {
        //[JsonIgnore]
        //public FormInputColumn InputColumn { get; set; }

        public string ColumnName { get; set; }

        public List<ValidRegularExpressionItem> ValidRegularExpressionItems { get; set; }
    }

    public class ValidRegularExpressionItem
    {
        public string RegularExpression { get; set; }

        public string ErrorMessage { get; set; }
    }
}
