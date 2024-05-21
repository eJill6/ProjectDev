using Serenity.ComponentModel;
using System.Collections.Generic;

namespace Management.Administration.Forms
{
    [FormScript("Administration.User")]
    [BasedOnRow(typeof(UserRow), CheckNames = true)]
    public class UserForm
    {
        [LabelWidth(200, UntilNext = true)]
        public string Username { get; set; }

        [LookupEditor(typeof(RoleRow), Multiple = true)]
        public List<int> Roles { get; set; }
     
        [PasswordEditor, Required(true)]
        public string Password { get; set; }
        [PasswordEditor, OneWay, Required(true)]
        public string PasswordConfirm { get; set; }
      
    }
}