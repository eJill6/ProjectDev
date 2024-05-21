using JxBackendService.Model.Enums.BackSideWeb.Common;

namespace JxBackendService.Model.BackSideWeb
{
    public class BackSideWebUserMessage
    {
        public BackSideWebUserActionTypes BackSideWebUserActionType { get; set; }

        public string Message { get; set; }
    }
}