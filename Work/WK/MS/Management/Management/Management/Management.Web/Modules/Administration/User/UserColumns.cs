using Serenity.ComponentModel;

namespace Management.Administration.Columns
{
    [ColumnsScript("Administration.User")]
    [BasedOnRow(typeof(UserRow), CheckNames = true)]
    public class UserColumns
    {
        [EditLink, Width(150)]
        public string Username { get; set; }

        [QuickFilter, Width(150)]
        public string Roles { get; set; }
    }
}
