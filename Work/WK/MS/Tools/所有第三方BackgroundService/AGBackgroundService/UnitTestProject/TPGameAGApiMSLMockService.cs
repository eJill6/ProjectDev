using AgDataBase.DLL.FileService;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.ThirdParty;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.ReturnModel;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.ThirdParty;
using JxBackendService.Service.ThirdPartyTransfer.MSL;
using ProductTransferService.AgDataBase.DLL.FileService;
using ProductTransferService.AgDataBase.Model;

namespace UnitTestProject
{
    public class TPGameAGApiMSLMockService : TPGameAGApiMSLService
    {
        public TPGameAGApiMSLMockService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        protected override BaseReturnDataModel<RequestAndResponse> GetRemoteBetLogApiResult(string lastSearchToken)
        {
            return base.GetRemoteBetLogApiResult(lastSearchToken);
        }

        protected override BaseReturnModel CheckOrCreateRemoteAccount(CreateRemoteAccountParam param)
        {
            return new BaseReturnModel(ReturnCode.Success);
        }

        protected override BaseReturnDataModel<string> GetRemoteCreateAccountApiResult(CreateRemoteAccountParam param)
        {
            return base.GetRemoteCreateAccountApiResult(param);
            //return "<?xml version=\"1.0\" encoding=\"utf-8\"?><result info=\"0\"  msg=\"\"/>";
        }

        protected override BaseReturnDataModel<string> GetRemoteLoginApiResult(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            return base.GetRemoteLoginApiResult(tpGameRemoteLoginParam);
        }

        protected override BaseReturnDataModel<string> GetRemoteCheckAccountExistApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            return new BaseReturnDataModel<string>(ReturnCode.Success,
                "<?xml version=\"1.0\" encoding=\"utf-8\"?><result info=\"null\"  msg=\"\"/>");
        }

        protected override DetailRequestAndResponse GetRemoteTransferApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            return base.GetRemoteTransferApiResult(isMoneyIn, createRemoteAccountParam, tpGameMoneyInfo);
        }

        protected override string GetRemoteUserScoreApiResult(CreateRemoteAccountParam createRemoteAccountParam)
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?><result info=\"500\"  msg=\"\"/>";
        }

        protected override BaseReturnDataModel<string> GetRemoteForwardGameUrl(TPGameRemoteLoginParam tpGameRemoteLoginParam)
        {
            return base.GetRemoteForwardGameUrl(tpGameRemoteLoginParam);
        }

        protected override BaseReturnDataModel<DetailRequestAndResponse> PrepareTransferCredit(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            string errorMsg = "account_add_fail";
            var detail = new DetailRequestAndResponse
            {
                RequestUrl = $"http://test.transfer.{PlatformProduct.AG.Value}",
            };

            return new BaseReturnDataModel<DetailRequestAndResponse>(errorMsg, detail);
        }

        protected override DetailRequestAndResponse GetTransferCreditConfirmApiResult(bool isMoneyIn, CreateRemoteAccountParam createRemoteAccountParam, BaseTPGameMoneyInfo tpGameMoneyInfo)
        {
            //return "<?xml version=\"1.0\" encoding=\"utf-8\"?><result info=\"0\"  msg=\"\"/>";
            return base.GetTransferCreditConfirmApiResult(isMoneyIn, createRemoteAccountParam, tpGameMoneyInfo);
        }
    }

    public class AGRemoteOssXmlFileMockService : IAGRemoteXmlFileService
    {
        public void DeleteRemoteFile(XMLFile xmlFile)
        {
            
        }

        public void DeleteRemoteFiles(List<XMLFile> downLoadXmlFiles)
        {
            
        }

        public void DownloadAllRemoteNormalXmlFiles(AGGameType agGameType)
        {
            
        }

        public void DownloadAllRemoteLostAndFoundXmlFiles(AGGameType agGameType)
        {
            
        }

        public List<XMLFile> GetAllLocalXmlFiles(AGGameType agGameType)
        {
            return new List<XMLFile>()
            {
                new XMLFile()
                {
                }
            };
        }
    }
}