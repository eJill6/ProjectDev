using JxBackendService.Resource.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxBackendService.Model.Enums.Resource
{
    public class ResourceInfo
    {
        public string Directory { get; private set; }

        public string ResourceName { get; private set; }

        private ResourceInfo() { }


        public static readonly ResourceInfo Audit = new ResourceInfo() { Directory = "Element", ResourceName = nameof(AuditElement) };
        public static readonly ResourceInfo ChatRoom = new ResourceInfo() { Directory = "Element", ResourceName = nameof(ChatRoomElement) };
        public static readonly ResourceInfo Commission = new ResourceInfo() { Directory = "Element", ResourceName = nameof(CommissionElement) };
        public static readonly ResourceInfo Common = new ResourceInfo() { Directory = "Element", ResourceName = nameof(CommonElement) };
        public static readonly ResourceInfo DBContent = new ResourceInfo() { Directory = "Element", ResourceName = nameof(DBContentElement) };
        public static readonly ResourceInfo ForgetPassword = new ResourceInfo() { Directory = "Element", ResourceName = nameof(ForgetPasswordElement) };
        public static readonly ResourceInfo Message = new ResourceInfo() { Directory = "Element", ResourceName = nameof(MessageElement) };
        public static readonly ResourceInfo OperationLogContent = new ResourceInfo() { Directory = "Element", ResourceName = nameof(OperationLogContentElement) };
        public static readonly ResourceInfo PlatformProduct = new ResourceInfo() { Directory = "Element", ResourceName = nameof(PlatformProductElement) };
        public static readonly ResourceInfo ReturnCode = new ResourceInfo() { Directory = "Element", ResourceName = nameof(ReturnCodeElement) };
        public static readonly ResourceInfo Security = new ResourceInfo() { Directory = "Element", ResourceName = nameof(SecurityElement) };
        public static readonly ResourceInfo SelectItem = new ResourceInfo() { Directory = "Element", ResourceName = nameof(SelectItemElement) };
        public static readonly ResourceInfo ThirdPartyGame = new ResourceInfo() { Directory = "Element", ResourceName = nameof(ThirdPartyGameElement) };
        public static readonly ResourceInfo UserAuth = new ResourceInfo() { Directory = "Element", ResourceName = nameof(UserAuthElement) };
        public static readonly ResourceInfo UserRelated = new ResourceInfo() { Directory = "Element", ResourceName = nameof(UserRelatedElement) };
        public static readonly ResourceInfo VIPContent = new ResourceInfo() { Directory = "Element", ResourceName = nameof(VIPContentElement) };
        public static readonly ResourceInfo Deposit = new ResourceInfo() { Directory = "Page", ResourceName = nameof(Deposit) };
    }
}
