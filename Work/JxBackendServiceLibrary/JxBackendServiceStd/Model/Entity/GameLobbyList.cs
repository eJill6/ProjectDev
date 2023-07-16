using JxBackendService.Model.Attributes;
using JxBackendService.Model.Entity.Base;

namespace JxBackendService.Model.Entity
{
    public class BaseGameLobbyList : BaseEntityModel
    {
        [IdentityKey]
        public int No { get; set; }

        public string ChineseName { get; set; }

        public string EnglishName { get; set; }

        public string WebGameCode { get; set; }

        public string MobileGameCode { get; set; }

        public bool IsHot { get; set; }

        public int Sort { get; set; }

        public string ImageUrl { get; set; }
    }

    public class GameLobbyList : BaseGameLobbyList
    {
        public string ThirtyPartyCode { get; set; }

        public bool IsActive { get; set; }
    }
}