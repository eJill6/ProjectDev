using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums.ResetPassword
{
    public class BaseResetPasswordType : BaseIntValueModel<BaseResetPasswordType>
    {
        private BaseResetPasswordType() { }

        public string AppInputTypeValue { get; protected set; }

        public static readonly BaseResetPasswordType MoneyPassword = new BaseResetPasswordType()
        {
            Value = 1,
            AppInputTypeValue = "username"
        };

        /// <summary>
        /// 手機找回目前已不支援，但怕未來會再恢復，先使用註解的方式
        /// </summary>
        //public static readonly BaseResetPasswordType Phone = new BaseResetPasswordType() 
        //{   
        //    Value = "phone", 
        //    IntTypeValue = 2 
        //};

        public static readonly BaseResetPasswordType Email = new BaseResetPasswordType()
        {
            Value = 3,
            AppInputTypeValue = "email"
        };

        public static readonly BaseResetPasswordType GoogleAuth = new BaseResetPasswordType()
        {
            Value = 4,
            AppInputTypeValue = "googleauth",
        };

        /// <summary>
        /// 容舊
        /// 用App傳入的Type換回共用的Value
        /// </summary>
        /// <param name="appInputTypeValue"></param>
        /// <returns></returns>
        public static BaseResetPasswordType GetSingleByAppInputTypeValue(string appInputTypeValue)
        {
            return BaseResetPasswordType.GetAll().SingleOrDefault(pt => pt.AppInputTypeValue == appInputTypeValue);
        }
    }

    public class ResetLoginPasswordType
    {
        private ResetLoginPasswordType() { }

        public static List<BaseResetPasswordType> ResetPasswordTypes => new List<BaseResetPasswordType>()
        {
            BaseResetPasswordType.MoneyPassword,
            BaseResetPasswordType.Email,
            BaseResetPasswordType.GoogleAuth
        };
    }

    public class ResetMoneyPasswordType
    {
        private ResetMoneyPasswordType() { }

        public static List<BaseResetPasswordType> ResetPasswordTypes => new List<BaseResetPasswordType>()
        {
            BaseResetPasswordType.Email,
            BaseResetPasswordType.GoogleAuth
        };
    }
}
