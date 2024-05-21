using Serenity.ComponentModel;
using Serenity.Data;
using Serenity.Data.Mapping;
using System;
using System.ComponentModel;

namespace Management.ProductManagement
{
    [ConnectionKey("Inlodb"), Module("ProductManagement"), TableName("[dbo].[FrontsideMenu]")]
    [DisplayName("Frontside Menu"), InstanceName("Frontside Menu")]
    [ReadPermission("ProductManagement:FrontsideMenu")]
    [ModifyPermission("ProductManagement:FrontsideMenu")]
    public sealed class FrontsideMenuRow : Row<FrontsideMenuRow.RowFields>, IIdRow, INameRow
    {
        [DisplayName("No"), Identity, IdProperty]
        public int? No
        {
            get => fields.No[this];
            set => fields.No[this] = value;
        }

        [DisplayName("Menu Name"), Size(50), NotNull, QuickSearch, NameProperty]
        public string MenuName
        {
            get => fields.MenuName[this];
            set => fields.MenuName[this] = value;
        }

        [DisplayName("Eng Name"), Size(50)]
        public string EngName
        {
            get => fields.EngName[this];
            set => fields.EngName[this] = value;
        }

        [DisplayName("Pic Name"), Size(50)]
        public string PicName
        {
            get => fields.PicName[this];
            set => fields.PicName[this] = value;
        }

        [DisplayName("Product Code"), Size(50), NotNull]
        public string ProductCode
        {
            get => fields.ProductCode[this];
            set => fields.ProductCode[this] = value;
        }

        [DisplayName("Game Code"), Size(50), NotNull]
        public string GameCode
        {
            get => fields.GameCode[this];
            set => fields.GameCode[this] = value;
        }

        [DisplayName("Type"), NotNull]
        public int? Type
        {
            get => fields.Type[this];
            set => fields.Type[this] = value;
        }

        [DisplayName("Sort"), NotNull]
        public int? Sort
        {
            get => fields.Sort[this];
            set => fields.Sort[this] = value;
        }

        [DisplayName("App Sort"), NotNull]
        public int? AppSort
        {
            get => fields.AppSort[this];
            set => fields.AppSort[this] = value;
        }

        [DisplayName("Url"), Size(200)]
        public string Url
        {
            get => fields.Url[this];
            set => fields.Url[this] = value;
        }

        [DisplayName("Is Active"), NotNull]
        public bool? IsActive
        {
            get => fields.IsActive[this];
            set => fields.IsActive[this] = value;
        }

        [DisplayName("Create Date"), NotNull]
        public DateTime? CreateDate
        {
            get => fields.CreateDate[this];
            set => fields.CreateDate[this] = value;
        }

        [DisplayName("Create User"), Size(50), NotNull]
        public string CreateUser
        {
            get => fields.CreateUser[this];
            set => fields.CreateUser[this] = value;
        }

        [DisplayName("Update Date")]
        public DateTime? UpdateDate
        {
            get => fields.UpdateDate[this];
            set => fields.UpdateDate[this] = value;
        }

        [DisplayName("Update User"), Size(50)]
        public string UpdateUser
        {
            get => fields.UpdateUser[this];
            set => fields.UpdateUser[this] = value;
        }

        public FrontsideMenuRow()
            : base()
        {
        }

        public FrontsideMenuRow(RowFields fields)
            : base(fields)
        {
        }

        public class RowFields : RowFieldsBase
        {
            public Int32Field No;
            public StringField MenuName;
            public StringField EngName;
            public StringField PicName;
            public StringField ProductCode;
            public StringField GameCode;
            public Int32Field Type;
            public Int32Field Sort;
            public Int32Field AppSort;
            public StringField Url;
            public BooleanField IsActive;
            public DateTimeField CreateDate;
            public StringField CreateUser;
            public DateTimeField UpdateDate;
            public StringField UpdateUser;
        }
    }
}