using System;
using JxBackendService.Model.ReturnModel;

namespace JxBackendService.Model.ViewModel
{
    public class SecurityToken
    {
        public int TokenTypeId { get; set; }
        
        public int UserId { get; set; }
        
        public int StepId { get; set; }
        
        public DateTime HandleDate { get; set; }

        public string Data { get; set; }
    }

    public class SaveUserNickname
    {
        public string AccessToken { get; set; }
        public string Nickname { get; set; }
    }

    public class SecurityNextStep : BaseBehaviorModel
    {
        public string AccessToken { get; set; }
    }

    public class SaveSecuritySetting
    {
        public string AccessToken { get; set; }
        public string MoneyPasswordHash { get; set; }
        public string Email { get; set; }
        public int FirstQuestionId { get; set; }
        public string FirstAnswer { get; set; }
        public int SecondQuestionId { get; set; }
        public string SecondAnswer { get; set; }
    }
}
