using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Service.SubGame;
using JxBackendService.Service.SubGame.IMOne;
using System;
using System.Linq;

namespace JxBackendService.Model.Enums
{
    /// <summary>
    /// 有大廳的遊戲列舉
    /// </summary>
    public class GameLobbyType : BaseStringValueModel<GameLobbyType>
    {
        public PlatformProduct Product { get; private set; }

        public string SubGameCode { get; private set; } = string.Empty;

        public Type SubGameServiceType { get; private set; }

        public bool IsSquareGameImage { get; private set; }

        public bool IsSelfOpenPage { get; set; }

        public bool IsLobbyShowJackpot { get; set; }

        public static GameLobbyType IMPT = new GameLobbyType()
        {
            Value = PlatformProduct.IMPT.Value,
            Product = PlatformProduct.IMPT,
            SubGameServiceType = typeof(IMPTSubGameService),
            SubGameCode = ThirdPartySubGameCodes.IMPT.Value,
            IsSelfOpenPage = true,
            IsLobbyShowJackpot = true,
        };

        public static GameLobbyType IMPP = new GameLobbyType()
        {
            Value = PlatformProduct.IMPP.Value,
            Product = PlatformProduct.IMPP,
            SubGameServiceType = typeof(IMPPSubGameService),
            SubGameCode = ThirdPartySubGameCodes.IMPP.Value,
        };

        public static GameLobbyType IMJDB = new GameLobbyType()
        {
            Value = ThirdPartySubGameCodes.IMJDB.Value,
            Product = PlatformProduct.IMPP,
            SubGameServiceType = typeof(IMJDBSubGameService),
            SubGameCode = ThirdPartySubGameCodes.IMJDB.Value,
            IsSquareGameImage = true,
        };

        public static GameLobbyType IMSE = new GameLobbyType()
        {
            Value = ThirdPartySubGameCodes.IMSE.Value,
            Product = PlatformProduct.IMPP,
            SubGameServiceType = typeof(IMSESubGameService),
            SubGameCode = ThirdPartySubGameCodes.IMSE.Value,
        };

        public static GameLobbyType JDBFI = new GameLobbyType()
        {
            Value = PlatformProduct.JDBFI.Value,
            Product = PlatformProduct.JDBFI,
            SubGameServiceType = typeof(JDBFISubGameService),
            IsSquareGameImage = true,
        };

        public static GameLobbyType PMSL = new GameLobbyType()
        {
            Value = PlatformProduct.PMSL.Value,
            Product = PlatformProduct.PMSL,
            SubGameServiceType = typeof(PMSLSubGameService),
        };

        public static bool IsGameLobby(string product, string subGameCode)
        {
            GameLobbyType lobbyType = GetSingle(product);

            if (lobbyType == null)
            {
                return false;
            }

            GameLobbyType gameLobbyType = GetGameLobbyType(product, subGameCode);

            if (gameLobbyType != null)
            {
                return true;
            }

            return false;
        }

        public static GameLobbyType GetGameLobbyType(string product, string subGameCode)
        {
            return GetAll().Where(w => w.Product.Value == product && w.SubGameCode == subGameCode).FirstOrDefault();
        }
    }
}