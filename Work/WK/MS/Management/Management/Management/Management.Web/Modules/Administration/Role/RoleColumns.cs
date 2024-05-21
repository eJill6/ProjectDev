using Serenity.ComponentModel;
using System;
using System.ComponentModel;

namespace Management.Administration.Forms
{
    [ColumnsScript("Administration.Role")]
    [BasedOnRow(typeof(RoleRow), CheckNames = true)]
    public class RoleColumns
    {
        [EditLink, Width(300)]
        public String RoleName { get; set; }
    }
}