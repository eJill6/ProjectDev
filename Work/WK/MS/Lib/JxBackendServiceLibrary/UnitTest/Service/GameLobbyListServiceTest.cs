using JxBackendService.Common.Util;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Interface.Service.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel.ThirdParty;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using UnitTest.Base;

namespace UnitTest.Util
{
    [TestClass]
    public class GameLobbyListServiceTest : BaseTest
    {
        private readonly Lazy<IGameLobbyListService> _gameLobbyListService;

        public GameLobbyListServiceTest()
        {
            _gameLobbyListService = DependencyUtil.ResolveJxBackendService<IGameLobbyListService>(EnvLoginUser, DbConnectionTypes.Slave);
        }

        [TestMethod]
        public void ActiveGameLobbyListTest()
        {
            List<GameLobbyInfo> gameLobbyInfos = _gameLobbyListService.Value.GetActiveGameLobbyList(GameLobbyType.JDBFI);
            DependencyUtil.ResolveService<ILogUtilService>().Value.ForcedDebug(gameLobbyInfos.ToJsonString());
        }
    }
}