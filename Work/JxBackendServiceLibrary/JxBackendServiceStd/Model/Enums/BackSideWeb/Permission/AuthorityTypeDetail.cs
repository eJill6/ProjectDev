using JxBackendService.Resource.Element;

namespace JxBackendService.Model.Enums.BackSideWeb.Permission
{
    public enum AuthorityTypes
    {
        Read = 1,

        Edit = 2,

        Delete = 3,
    }

    public class AuthorityTypeDetail : BaseIntValueModel<AuthorityTypeDetail>
    {
        private AuthorityTypeDetail()
        {
            ResourceType = typeof(PermissionElement);
        }

        public static AuthorityTypeDetail Read = new AuthorityTypeDetail()
        {
            Value = (int)AuthorityTypes.Read,
            ResourcePropertyName = nameof(PermissionElement.Read),
        };

        public static AuthorityTypeDetail Edit = new AuthorityTypeDetail()
        {
            Value = (int)AuthorityTypes.Edit,
            ResourcePropertyName = nameof(PermissionElement.InsertOrEdit),
        };

        public static AuthorityTypeDetail Delete = new AuthorityTypeDetail()
        {
            Value = (int)AuthorityTypes.Delete,
            ResourcePropertyName = nameof(PermissionElement.Delete),
        };
    }
}