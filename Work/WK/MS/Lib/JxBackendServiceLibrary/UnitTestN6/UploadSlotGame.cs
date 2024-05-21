using Castle.Core.Internal;
using JxBackendService.DependencyInjection;
using JxBackendService.Interface.Service.Config;
using JxBackendService.Interface.Service.Game;
using JxBackendService.Interface.Service.OSS;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.Game;
using JxBackendService.Model.Param.OSS;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.Game;
using JxBackendServiceN6.Common.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnitTestN6;

namespace UnitTest.OSS
{
    [TestClass]
    public class UploadSlotGame : BaseUnitTest
    {
        public static readonly List<string> s_awsBucketNames = new List<string>
        {
            "youqu.msl.development",
            "youqu.msl.sit",
            "youqu.msl.uat",
            "youqu.msl.production"
        };

        public readonly List<IObjectStorageService> ossServices = new List<IObjectStorageService>();


        private EnvironmentUser EnvLoginUser => new EnvironmentUser { Application = Application };

        public UploadSlotGame()
        {
            var ossSettingService = DependencyUtil.ResolveJxBackendService<IOSSSettingService>(EnvLoginUser, DbConnectionTypes.Slave).Value;
            IAmazonS3Setting setting = ossSettingService.GetOSSClientSetting() as IAmazonS3Setting;

            foreach (string bucketName in s_awsBucketNames)
            {
                IOSSSetting customSetting = new AmazonS3Setting()
                {
                    AccessKeyId = setting.AccessKeyId,
                    AccessKeySecret = setting.AccessKeySecret,
                    Endpoint = setting.Endpoint,
                    RegionEndpoint = setting.RegionEndpoint,
                    BucketName = bucketName,
                };

                var ossService = DependencyUtil.ResolveServiceForModel<IObjectStorageService>(customSetting).Value;

                ossServices.Add(ossService);
            }
        }

        [TestMethod]
        public void ByteUploadObject()
        {
            try
            {
                var slotGameManageReadService = DependencyUtil.ResolveJxBackendService<ISlotGameManageReadService>(EnvLoginUser, DbConnectionTypes.Slave).Value;
                PagedResultModel<SlotGameManageModel> pagedModel = slotGameManageReadService.GetPagedModel(new SlotGameManageQueryParam
                {
                    PageNo = 1,
                    PageSize = 10000,
                });

                foreach (SlotGameManageModel model in pagedModel.ResultList)
                {
                    if (model.GameCode.IsNullOrEmpty())
                    {
                        continue;
                    }

                    GameLobbyType type = GameLobbyType.GetSingle(model.ThirdPartyCode);
                    string webContentPath = $"..\\..\\..\\..\\..\\..\\Web_WebSV\\Web\\Web\\Content\\images\\{model.ThirdPartyCode}\\GameList";
                    string fileName = $"{model.GameCode}.png";
                    string imageFile = Directory.GetFiles(webContentPath, fileName).Single();
                    byte[] imageData = File.ReadAllBytes(imageFile);

                    UploadToOSS(imageData, model.ImageUrl);

                    Console.WriteLine($"Put image {fileName} object succeeded");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Put object failed, {0}", ex.Message);
                throw;
            }
        }

        private void UploadToOSS(byte[] bytes, string imageUrl)
        {
            foreach (var ossService in ossServices)
            {
                ossService.UploadObject(bytes, imageUrl);
            }
        }

        [TestMethod]
        public void GetList()
        {
            var devOssService = ossServices[0];
            List<string> filenames = devOssService.GetFullFileNames("BackSideWeb/Upload/SlotGames");

            foreach (string filename in filenames)
            {
                LogUtil.ForcedDebug(filename);
            }

            LogUtil.ForcedDebug($"{filenames.Count}\n");
        }
    }
}