namespace JxBackendService.Model.Enums.BackSideWeb.Permission
{
    public static class AuthoritySetting
    {
        public static AuthorityTypeDetail[] Read()
        {
            return new AuthorityTypeDetail[] { AuthorityTypeDetail.Read };
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