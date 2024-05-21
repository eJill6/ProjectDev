using Serenity.ComponentModel;
using Serenity.Services;
using System.ComponentModel;

namespace Management.Membership
{
    [FormScript("Membership.Login")]
    //[BasedOnRow(typeof(Administration.UserRow), CheckNames = true)]
    public class LoginRequest : ServiceRequest
    {
        //[Placeholder("user name")]
        //public string Username { get; set; }
        //[PasswordEditor, Required(true), Placeholder("password")]
        //public string Password { get; set; }

        [Required, DisplayName("请输入验证码")]
        public string Token { get; set; }
    }
}