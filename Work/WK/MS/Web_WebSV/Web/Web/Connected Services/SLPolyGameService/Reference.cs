﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.SLPolyGameService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FrontsideMenuViewModel", Namespace="http://schemas.datacontract.org/2004/07/JxBackendService.Model.ViewModel")]
    [System.SerializableAttribute()]
    public partial class FrontsideMenuViewModel : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<Web.SLPolyGameService.FrontsideMenuTypeViewModel> FrontsideMenuTypesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<Web.SLPolyGameService.PagedFrontsideProductMenu> PagedFrontsideProductMenusField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<Web.SLPolyGameService.FrontsideMenuTypeViewModel> FrontsideMenuTypes {
            get {
                return this.FrontsideMenuTypesField;
            }
            set {
                if ((object.ReferenceEquals(this.FrontsideMenuTypesField, value) != true)) {
                    this.FrontsideMenuTypesField = value;
                    this.RaisePropertyChanged("FrontsideMenuTypes");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<Web.SLPolyGameService.PagedFrontsideProductMenu> PagedFrontsideProductMenus {
            get {
                return this.PagedFrontsideProductMenusField;
            }
            set {
                if ((object.ReferenceEquals(this.PagedFrontsideProductMenusField, value) != true)) {
                    this.PagedFrontsideProductMenusField = value;
                    this.RaisePropertyChanged("PagedFrontsideProductMenus");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FrontsideMenuTypeViewModel", Namespace="http://schemas.datacontract.org/2004/07/JxBackendService.Model.ViewModel")]
    [System.SerializableAttribute()]
    public partial class FrontsideMenuTypeViewModel : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CardOutCssClassField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ColsInRowField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IconFileNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MaintainanceCssClassField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MenuTypeNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int MenuTypeValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CardOutCssClass {
            get {
                return this.CardOutCssClassField;
            }
            set {
                if ((object.ReferenceEquals(this.CardOutCssClassField, value) != true)) {
                    this.CardOutCssClassField = value;
                    this.RaisePropertyChanged("CardOutCssClass");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ColsInRow {
            get {
                return this.ColsInRowField;
            }
            set {
                if ((this.ColsInRowField.Equals(value) != true)) {
                    this.ColsInRowField = value;
                    this.RaisePropertyChanged("ColsInRow");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IconFileName {
            get {
                return this.IconFileNameField;
            }
            set {
                if ((object.ReferenceEquals(this.IconFileNameField, value) != true)) {
                    this.IconFileNameField = value;
                    this.RaisePropertyChanged("IconFileName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MaintainanceCssClass {
            get {
                return this.MaintainanceCssClassField;
            }
            set {
                if ((object.ReferenceEquals(this.MaintainanceCssClassField, value) != true)) {
                    this.MaintainanceCssClassField = value;
                    this.RaisePropertyChanged("MaintainanceCssClass");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MenuTypeName {
            get {
                return this.MenuTypeNameField;
            }
            set {
                if ((object.ReferenceEquals(this.MenuTypeNameField, value) != true)) {
                    this.MenuTypeNameField = value;
                    this.RaisePropertyChanged("MenuTypeName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int MenuTypeValue {
            get {
                return this.MenuTypeValueField;
            }
            set {
                if ((this.MenuTypeValueField.Equals(value) != true)) {
                    this.MenuTypeValueField = value;
                    this.RaisePropertyChanged("MenuTypeValue");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PagedFrontsideProductMenu", Namespace="http://schemas.datacontract.org/2004/07/JxBackendService.Model.ViewModel")]
    [System.SerializableAttribute()]
    public partial class PagedFrontsideProductMenu : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.List<Web.SLPolyGameService.FrontsideProductMenu> FrontsideProductMenusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int MenuTypeValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<Web.SLPolyGameService.FrontsideProductMenu> FrontsideProductMenus {
            get {
                return this.FrontsideProductMenusField;
            }
            set {
                if ((object.ReferenceEquals(this.FrontsideProductMenusField, value) != true)) {
                    this.FrontsideProductMenusField = value;
                    this.RaisePropertyChanged("FrontsideProductMenus");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int MenuTypeValue {
            get {
                return this.MenuTypeValueField;
            }
            set {
                if ((this.MenuTypeValueField.Equals(value) != true)) {
                    this.MenuTypeValueField = value;
                    this.RaisePropertyChanged("MenuTypeValue");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FrontsideProductMenu", Namespace="http://schemas.datacontract.org/2004/07/JxBackendService.Model.ViewModel")]
    [System.SerializableAttribute()]
    public partial class FrontsideProductMenu : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CardCssClassField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FullImageUrlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string GameCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsHideHeaderWithFullScreenField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsMaintainingField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsRedirectUrlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProductCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ProductNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RemoteCodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TitleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UrlField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CardCssClass {
            get {
                return this.CardCssClassField;
            }
            set {
                if ((object.ReferenceEquals(this.CardCssClassField, value) != true)) {
                    this.CardCssClassField = value;
                    this.RaisePropertyChanged("CardCssClass");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FullImageUrl {
            get {
                return this.FullImageUrlField;
            }
            set {
                if ((object.ReferenceEquals(this.FullImageUrlField, value) != true)) {
                    this.FullImageUrlField = value;
                    this.RaisePropertyChanged("FullImageUrl");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string GameCode {
            get {
                return this.GameCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.GameCodeField, value) != true)) {
                    this.GameCodeField = value;
                    this.RaisePropertyChanged("GameCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsHideHeaderWithFullScreen {
            get {
                return this.IsHideHeaderWithFullScreenField;
            }
            set {
                if ((this.IsHideHeaderWithFullScreenField.Equals(value) != true)) {
                    this.IsHideHeaderWithFullScreenField = value;
                    this.RaisePropertyChanged("IsHideHeaderWithFullScreen");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsMaintaining {
            get {
                return this.IsMaintainingField;
            }
            set {
                if ((this.IsMaintainingField.Equals(value) != true)) {
                    this.IsMaintainingField = value;
                    this.RaisePropertyChanged("IsMaintaining");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsRedirectUrl {
            get {
                return this.IsRedirectUrlField;
            }
            set {
                if ((this.IsRedirectUrlField.Equals(value) != true)) {
                    this.IsRedirectUrlField = value;
                    this.RaisePropertyChanged("IsRedirectUrl");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProductCode {
            get {
                return this.ProductCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.ProductCodeField, value) != true)) {
                    this.ProductCodeField = value;
                    this.RaisePropertyChanged("ProductCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ProductName {
            get {
                return this.ProductNameField;
            }
            set {
                if ((object.ReferenceEquals(this.ProductNameField, value) != true)) {
                    this.ProductNameField = value;
                    this.RaisePropertyChanged("ProductName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RemoteCode {
            get {
                return this.RemoteCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.RemoteCodeField, value) != true)) {
                    this.RemoteCodeField = value;
                    this.RaisePropertyChanged("RemoteCode");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Title {
            get {
                return this.TitleField;
            }
            set {
                if ((object.ReferenceEquals(this.TitleField, value) != true)) {
                    this.TitleField = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Url {
            get {
                return this.UrlField;
            }
            set {
                if ((object.ReferenceEquals(this.UrlField, value) != true)) {
                    this.UrlField = value;
                    this.RaisePropertyChanged("Url");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SLPolyGameService.ISLPolyGameService")]
    public interface ISLPolyGameService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/CancelOrder", ReplyAction="http://tempuri.org/ISLPolyGameService/CancelOrderResponse")]
        string CancelOrder(SLPolyGame.Web.Model.PalyInfo model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/CancelOrder", ReplyAction="http://tempuri.org/ISLPolyGameService/CancelOrderResponse")]
        System.Threading.Tasks.Task<string> CancelOrderAsync(SLPolyGame.Web.Model.PalyInfo model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetUserInfoByUserID", ReplyAction="http://tempuri.org/ISLPolyGameService/GetUserInfoByUserIDResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SLPolyGame.Web.Model.UserAuthInformation))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SLPolyGame.Web.Model.LoginRequestParam))]
        SLPolyGame.Web.Model.UserInfo GetUserInfoByUserID(int UserID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetUserInfoByUserID", ReplyAction="http://tempuri.org/ISLPolyGameService/GetUserInfoByUserIDResponse")]
        System.Threading.Tasks.Task<SLPolyGame.Web.Model.UserInfo> GetUserInfoByUserIDAsync(int UserID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/ValidateLogin", ReplyAction="http://tempuri.org/ISLPolyGameService/ValidateLoginResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SLPolyGame.Web.Model.LoginRequestParam))]
        SLPolyGame.Web.Model.MessageEntity<SLPolyGame.Web.Model.UserAuthInformation> ValidateLogin(SLPolyGame.Web.Model.LoginRequestParam param);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/ValidateLogin", ReplyAction="http://tempuri.org/ISLPolyGameService/ValidateLoginResponse")]
        System.Threading.Tasks.Task<SLPolyGame.Web.Model.MessageEntity<SLPolyGame.Web.Model.UserAuthInformation>> ValidateLoginAsync(SLPolyGame.Web.Model.LoginRequestParam param);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetServerCurrentTime", ReplyAction="http://tempuri.org/ISLPolyGameService/GetServerCurrentTimeResponse")]
        System.DateTime GetServerCurrentTime();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetServerCurrentTime", ReplyAction="http://tempuri.org/ISLPolyGameService/GetServerCurrentTimeResponse")]
        System.Threading.Tasks.Task<System.DateTime> GetServerCurrentTimeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetSysSettings", ReplyAction="http://tempuri.org/ISLPolyGameService/GetSysSettingsResponse")]
        SLPolyGame.Web.Model.SysSettings GetSysSettings();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetSysSettings", ReplyAction="http://tempuri.org/ISLPolyGameService/GetSysSettingsResponse")]
        System.Threading.Tasks.Task<SLPolyGame.Web.Model.SysSettings> GetSysSettingsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/InsertPlayInfo", ReplyAction="http://tempuri.org/ISLPolyGameService/InsertPlayInfoResponse")]
        SLPolyGame.Web.Model.PalyInfo InsertPlayInfo(SLPolyGame.Web.Model.PalyInfo model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/InsertPlayInfo", ReplyAction="http://tempuri.org/ISLPolyGameService/InsertPlayInfoResponse")]
        System.Threading.Tasks.Task<SLPolyGame.Web.Model.PalyInfo> InsertPlayInfoAsync(SLPolyGame.Web.Model.PalyInfo model);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetPalyIDPalyBet", ReplyAction="http://tempuri.org/ISLPolyGameService/GetPalyIDPalyBetResponse")]
        SLPolyGame.Web.Model.PalyInfo GetPalyIDPalyBet(string value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetPalyIDPalyBet", ReplyAction="http://tempuri.org/ISLPolyGameService/GetPalyIDPalyBetResponse")]
        System.Threading.Tasks.Task<SLPolyGame.Web.Model.PalyInfo> GetPalyIDPalyBetAsync(string value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetCursorPaginationDrawResult", ReplyAction="http://tempuri.org/ISLPolyGameService/GetCursorPaginationDrawResultResponse")]
        SLPolyGame.Web.Model.CursorPagination<SLPolyGame.Web.Model.CurrentLotteryInfo> GetCursorPaginationDrawResult(int lotteryId, System.DateTime start, System.DateTime end, int count, string cursor);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetCursorPaginationDrawResult", ReplyAction="http://tempuri.org/ISLPolyGameService/GetCursorPaginationDrawResultResponse")]
        System.Threading.Tasks.Task<SLPolyGame.Web.Model.CursorPagination<SLPolyGame.Web.Model.CurrentLotteryInfo>> GetCursorPaginationDrawResultAsync(int lotteryId, System.DateTime start, System.DateTime end, int count, string cursor);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetFollowBet", ReplyAction="http://tempuri.org/ISLPolyGameService/GetFollowBetResponse")]
        SLPolyGame.Web.Model.CursorPagination<SLPolyGame.Web.Model.PalyInfo> GetFollowBet(string palyId, int lottertId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetFollowBet", ReplyAction="http://tempuri.org/ISLPolyGameService/GetFollowBetResponse")]
        System.Threading.Tasks.Task<SLPolyGame.Web.Model.CursorPagination<SLPolyGame.Web.Model.PalyInfo>> GetFollowBetAsync(string palyId, int lottertId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetSpecifyOrderList", ReplyAction="http://tempuri.org/ISLPolyGameService/GetSpecifyOrderListResponse")]
        SLPolyGame.Web.Model.CursorPagination<SLPolyGame.Web.Model.PalyInfo> GetSpecifyOrderList(int userId, int lotteryId, string status, System.DateTime searchDate, string cursor, int pageSize);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetSpecifyOrderList", ReplyAction="http://tempuri.org/ISLPolyGameService/GetSpecifyOrderListResponse")]
        System.Threading.Tasks.Task<SLPolyGame.Web.Model.CursorPagination<SLPolyGame.Web.Model.PalyInfo>> GetSpecifyOrderListAsync(int userId, int lotteryId, string status, System.DateTime searchDate, string cursor, int pageSize);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetUserInfo", ReplyAction="http://tempuri.org/ISLPolyGameService/GetUserInfoResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SLPolyGame.Web.Model.UserAuthInformation))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(SLPolyGame.Web.Model.LoginRequestParam))]
        SLPolyGame.Web.Model.UserInfo GetUserInfo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetUserInfo", ReplyAction="http://tempuri.org/ISLPolyGameService/GetUserInfoResponse")]
        System.Threading.Tasks.Task<SLPolyGame.Web.Model.UserInfo> GetUserInfoAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetLatestWinningListItems", ReplyAction="http://tempuri.org/ISLPolyGameService/GetLatestWinningListItemsResponse")]
        System.Collections.Generic.List<SLPolyGame.Web.Model.WinningListItem> GetLatestWinningListItems(string period);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetLatestWinningListItems", ReplyAction="http://tempuri.org/ISLPolyGameService/GetLatestWinningListItemsResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<SLPolyGame.Web.Model.WinningListItem>> GetLatestWinningListItemsAsync(string period);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetLatestWinningList", ReplyAction="http://tempuri.org/ISLPolyGameService/GetLatestWinningListResponse")]
        System.Collections.Generic.List<string> GetLatestWinningList(string period);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetLatestWinningList", ReplyAction="http://tempuri.org/ISLPolyGameService/GetLatestWinningListResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetLatestWinningListAsync(string period);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetPlayBetsByAnonymous", ReplyAction="http://tempuri.org/ISLPolyGameService/GetPlayBetsByAnonymousResponse")]
        System.Collections.Generic.List<SLPolyGame.Web.Model.PalyInfo> GetPlayBetsByAnonymous(string startTime, string endTime, string gameId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetPlayBetsByAnonymous", ReplyAction="http://tempuri.org/ISLPolyGameService/GetPlayBetsByAnonymousResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.List<SLPolyGame.Web.Model.PalyInfo>> GetPlayBetsByAnonymousAsync(string startTime, string endTime, string gameId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetPlayBetByAnonymous", ReplyAction="http://tempuri.org/ISLPolyGameService/GetPlayBetByAnonymousResponse")]
        SLPolyGame.Web.Model.PalyInfo GetPlayBetByAnonymous(string playId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetPlayBetByAnonymous", ReplyAction="http://tempuri.org/ISLPolyGameService/GetPlayBetByAnonymousResponse")]
        System.Threading.Tasks.Task<SLPolyGame.Web.Model.PalyInfo> GetPlayBetByAnonymousAsync(string playId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetFrontsideMenuViewModel", ReplyAction="http://tempuri.org/ISLPolyGameService/GetFrontsideMenuViewModelResponse")]
        Web.SLPolyGameService.FrontsideMenuViewModel GetFrontsideMenuViewModel();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/GetFrontsideMenuViewModel", ReplyAction="http://tempuri.org/ISLPolyGameService/GetFrontsideMenuViewModelResponse")]
        System.Threading.Tasks.Task<Web.SLPolyGameService.FrontsideMenuViewModel> GetFrontsideMenuViewModelAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/IsFrontsideMenuActive", ReplyAction="http://tempuri.org/ISLPolyGameService/IsFrontsideMenuActiveResponse")]
        bool IsFrontsideMenuActive(string productCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISLPolyGameService/IsFrontsideMenuActive", ReplyAction="http://tempuri.org/ISLPolyGameService/IsFrontsideMenuActiveResponse")]
        System.Threading.Tasks.Task<bool> IsFrontsideMenuActiveAsync(string productCode);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISLPolyGameServiceChannel : Web.SLPolyGameService.ISLPolyGameService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SLPolyGameServiceClient : System.ServiceModel.ClientBase<Web.SLPolyGameService.ISLPolyGameService>, Web.SLPolyGameService.ISLPolyGameService {
        
        public SLPolyGameServiceClient() {
        }
        
        public SLPolyGameServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SLPolyGameServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SLPolyGameServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SLPolyGameServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string CancelOrder(SLPolyGame.Web.Model.PalyInfo model) {
            return base.Channel.CancelOrder(model);
        }
        
        public System.Threading.Tasks.Task<string> CancelOrderAsync(SLPolyGame.Web.Model.PalyInfo model) {
            return base.Channel.CancelOrderAsync(model);
        }
        
        public SLPolyGame.Web.Model.UserInfo GetUserInfoByUserID(int UserID) {
            return base.Channel.GetUserInfoByUserID(UserID);
        }
        
        public System.Threading.Tasks.Task<SLPolyGame.Web.Model.UserInfo> GetUserInfoByUserIDAsync(int UserID) {
            return base.Channel.GetUserInfoByUserIDAsync(UserID);
        }
        
        public SLPolyGame.Web.Model.MessageEntity<SLPolyGame.Web.Model.UserAuthInformation> ValidateLogin(SLPolyGame.Web.Model.LoginRequestParam param) {
            return base.Channel.ValidateLogin(param);
        }
        
        public System.Threading.Tasks.Task<SLPolyGame.Web.Model.MessageEntity<SLPolyGame.Web.Model.UserAuthInformation>> ValidateLoginAsync(SLPolyGame.Web.Model.LoginRequestParam param) {
            return base.Channel.ValidateLoginAsync(param);
        }
        
        public System.DateTime GetServerCurrentTime() {
            return base.Channel.GetServerCurrentTime();
        }
        
        public System.Threading.Tasks.Task<System.DateTime> GetServerCurrentTimeAsync() {
            return base.Channel.GetServerCurrentTimeAsync();
        }
        
        public SLPolyGame.Web.Model.SysSettings GetSysSettings() {
            return base.Channel.GetSysSettings();
        }
        
        public System.Threading.Tasks.Task<SLPolyGame.Web.Model.SysSettings> GetSysSettingsAsync() {
            return base.Channel.GetSysSettingsAsync();
        }
        
        public SLPolyGame.Web.Model.PalyInfo InsertPlayInfo(SLPolyGame.Web.Model.PalyInfo model) {
            return base.Channel.InsertPlayInfo(model);
        }
        
        public System.Threading.Tasks.Task<SLPolyGame.Web.Model.PalyInfo> InsertPlayInfoAsync(SLPolyGame.Web.Model.PalyInfo model) {
            return base.Channel.InsertPlayInfoAsync(model);
        }
        
        public SLPolyGame.Web.Model.PalyInfo GetPalyIDPalyBet(string value) {
            return base.Channel.GetPalyIDPalyBet(value);
        }
        
        public System.Threading.Tasks.Task<SLPolyGame.Web.Model.PalyInfo> GetPalyIDPalyBetAsync(string value) {
            return base.Channel.GetPalyIDPalyBetAsync(value);
        }
        
        public SLPolyGame.Web.Model.CursorPagination<SLPolyGame.Web.Model.CurrentLotteryInfo> GetCursorPaginationDrawResult(int lotteryId, System.DateTime start, System.DateTime end, int count, string cursor) {
            return base.Channel.GetCursorPaginationDrawResult(lotteryId, start, end, count, cursor);
        }
        
        public System.Threading.Tasks.Task<SLPolyGame.Web.Model.CursorPagination<SLPolyGame.Web.Model.CurrentLotteryInfo>> GetCursorPaginationDrawResultAsync(int lotteryId, System.DateTime start, System.DateTime end, int count, string cursor) {
            return base.Channel.GetCursorPaginationDrawResultAsync(lotteryId, start, end, count, cursor);
        }
        
        public SLPolyGame.Web.Model.CursorPagination<SLPolyGame.Web.Model.PalyInfo> GetFollowBet(string palyId, int lottertId) {
            return base.Channel.GetFollowBet(palyId, lottertId);
        }
        
        public System.Threading.Tasks.Task<SLPolyGame.Web.Model.CursorPagination<SLPolyGame.Web.Model.PalyInfo>> GetFollowBetAsync(string palyId, int lottertId) {
            return base.Channel.GetFollowBetAsync(palyId, lottertId);
        }
        
        public SLPolyGame.Web.Model.CursorPagination<SLPolyGame.Web.Model.PalyInfo> GetSpecifyOrderList(int userId, int lotteryId, string status, System.DateTime searchDate, string cursor, int pageSize) {
            return base.Channel.GetSpecifyOrderList(userId, lotteryId, status, searchDate, cursor, pageSize);
        }
        
        public System.Threading.Tasks.Task<SLPolyGame.Web.Model.CursorPagination<SLPolyGame.Web.Model.PalyInfo>> GetSpecifyOrderListAsync(int userId, int lotteryId, string status, System.DateTime searchDate, string cursor, int pageSize) {
            return base.Channel.GetSpecifyOrderListAsync(userId, lotteryId, status, searchDate, cursor, pageSize);
        }
        
        public SLPolyGame.Web.Model.UserInfo GetUserInfo() {
            return base.Channel.GetUserInfo();
        }
        
        public System.Threading.Tasks.Task<SLPolyGame.Web.Model.UserInfo> GetUserInfoAsync() {
            return base.Channel.GetUserInfoAsync();
        }
        
        public System.Collections.Generic.List<SLPolyGame.Web.Model.WinningListItem> GetLatestWinningListItems(string period) {
            return base.Channel.GetLatestWinningListItems(period);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<SLPolyGame.Web.Model.WinningListItem>> GetLatestWinningListItemsAsync(string period) {
            return base.Channel.GetLatestWinningListItemsAsync(period);
        }
        
        public System.Collections.Generic.List<string> GetLatestWinningList(string period) {
            return base.Channel.GetLatestWinningList(period);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<string>> GetLatestWinningListAsync(string period) {
            return base.Channel.GetLatestWinningListAsync(period);
        }
        
        public System.Collections.Generic.List<SLPolyGame.Web.Model.PalyInfo> GetPlayBetsByAnonymous(string startTime, string endTime, string gameId) {
            return base.Channel.GetPlayBetsByAnonymous(startTime, endTime, gameId);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.List<SLPolyGame.Web.Model.PalyInfo>> GetPlayBetsByAnonymousAsync(string startTime, string endTime, string gameId) {
            return base.Channel.GetPlayBetsByAnonymousAsync(startTime, endTime, gameId);
        }
        
        public SLPolyGame.Web.Model.PalyInfo GetPlayBetByAnonymous(string playId) {
            return base.Channel.GetPlayBetByAnonymous(playId);
        }
        
        public System.Threading.Tasks.Task<SLPolyGame.Web.Model.PalyInfo> GetPlayBetByAnonymousAsync(string playId) {
            return base.Channel.GetPlayBetByAnonymousAsync(playId);
        }
        
        public Web.SLPolyGameService.FrontsideMenuViewModel GetFrontsideMenuViewModel() {
            return base.Channel.GetFrontsideMenuViewModel();
        }
        
        public System.Threading.Tasks.Task<Web.SLPolyGameService.FrontsideMenuViewModel> GetFrontsideMenuViewModelAsync() {
            return base.Channel.GetFrontsideMenuViewModelAsync();
        }
        
        public bool IsFrontsideMenuActive(string productCode) {
            return base.Channel.IsFrontsideMenuActive(productCode);
        }
        
        public System.Threading.Tasks.Task<bool> IsFrontsideMenuActiveAsync(string productCode) {
            return base.Channel.IsFrontsideMenuActiveAsync(productCode);
        }
    }
}