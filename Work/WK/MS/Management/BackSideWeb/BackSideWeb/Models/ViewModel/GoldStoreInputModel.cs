using BackSideWeb.Model.Entity.MM;
using JxBackendService.Model.Entity.Base;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BackSideWeb.Models.ViewModel
{
    public class GoldStoreInputModel : BaseEntityModel
    {
        public MMGoldStoreBs MMGoldStoreBs { get; set; }

        public string[]? TopUserIds { get; set; }

        public int Type { get; set; }
    }
}