namespace JxBackendService.Model.Enums.BackSideWeb.Permission
{
    public static class AuthoritySetting
    {
        public static AuthorityTypeDetail[] All()
        {
            return AuthorityTypeDetail.GetAll().ToArray();
        }

        public static AuthorityTypeDetail[] Standard()
        {
            return new AuthorityTypeDetail[] { AuthorityTypeDetail.Read, AuthorityTypeDetail.Edit, AuthorityTypeDetail.Delete };
        }

        public static AuthorityTypeDetail[] Read()
        {
            return new AuthorityTypeDetail[] { AuthorityTypeDetail.Read };
        }

        public static AuthorityTypeDetail[] ReadExport()
        {
            return new AuthorityTypeDetail[] { AuthorityTypeDetail.Read, AuthorityTypeDetail.Export };
        }

        public static AuthorityTypeDetail[] ReadEdit()
        {
            return new AuthorityTypeDetail[] { AuthorityTypeDetail.Read, AuthorityTypeDetail.Edit };
        }

        public static AuthorityTypeDetail[] ReadDelete()
        {
            return new AuthorityTypeDetail[] { AuthorityTypeDetail.Read, AuthorityTypeDetail.Delete };
        }
    }
}