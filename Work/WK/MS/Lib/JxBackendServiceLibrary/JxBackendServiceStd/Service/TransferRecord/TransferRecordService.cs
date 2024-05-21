using JxBackendService.Common.Util;
using JxBackendService.Interface.Repository.Finance;
using JxBackendService.Interface.Repository.TransferRecord;
using JxBackendService.Interface.Service.Enums;
using JxBackendService.Interface.Service.TransferRecord;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using JxBackendService.Model.Enums.Finance;
using JxBackendService.Model.Paging;
using JxBackendService.Model.Param.ThirdParty;
using JxBackendService.Model.Param.TransferRecord;
using JxBackendService.Model.ViewModel;
using JxBackendService.Model.ViewModel.TransferRecord;
using JxBackendService.Resource.Element;
using JxBackendService.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JxBackendService.Service.TransferRecord
{
    public class TransferRecordService : BaseService, ITransferRecordService
    {
        private readonly Lazy<IAllTPTransferRecordRep> _allTPTransferRecordRep;

        private readonly Lazy<IPlatformProductService> _platformProductService;

        private readonly string _platformName = "中继平台";

        private readonly string _miseName = "秘色";

        public TransferRecordService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _allTPTransferRecordRep = ResolveJxBackendService<IAllTPTransferRecordRep>();
            _platformProductService = ResolveKeyed<IPlatformProductService>(envLoginUser.Application);
        }

        public List<JxBackendSelectListItem> GetProductSelectListItems()
        {
            List<PlatformProduct> nonSelfProducts = _platformProductService.Value.GetNonSelfProduct();

            List<JxBackendSelectListItem> items = PlatformProduct.GetSelectListItems(nonSelfProducts, hasBlankOption: true, defaultDisplayText: SelectItemElement.All, defaultValue: null);

            return items;
        }

        public PagedResultModel<TransferRecordViewModel> GetTransferRecord(SearchTransferRecordParam param)
        {
            var result = new PagedResultModel<TransferRecordViewModel>()
            {
                PageNo = param.PageNo,
                PageSize = param.PageSize
            };

            if (!param.UserID.HasValue && param.ProductCode.IsNullOrEmpty())
            {
                //需输入用户ID或指定产品查询
                return result;
            }

            List<SearchTransferType> searchTransferTypes;

            if (param.TransferType.HasValue)
            {
                searchTransferTypes = new List<SearchTransferType> { SearchTransferType.GetSingle(param.TransferType.Value) };
            }
            else
            {
                searchTransferTypes = SearchTransferType.GetAll();
            }

            QueryTPTransferRecordParam queryTPParam = param.CastByJson<QueryTPTransferRecordParam>();
            QueryPlatformTransferRecordParam queryPlatformParam = param.CastByJson<QueryPlatformTransferRecordParam>();

            if (param.ProductCode.IsNullOrEmpty())
            {
                queryTPParam.PlatformProducts = _platformProductService.Value.GetNonSelfProduct();
            }
            else
            {
                PlatformProduct product = _platformProductService.Value.GetSingle(param.ProductCode);
                queryTPParam.PlatformProducts = new List<PlatformProduct> { product };
                queryPlatformParam.PlatformProduct = product;
            }

            PagedResultModel<TransferRecordViewModel> tpTransferRecord = _allTPTransferRecordRep.Value.GetAllTPTransferRecord(
                queryTPParam, searchTransferTypes, _platformName);

            tpTransferRecord.ResultList.ForEach(r => r.StatusText = TransferRecordOrderStatus.GetName(r.Status));
            result = ConcatPagedResultModel(result, tpTransferRecord);

            PagedResultModel<TransferRecordViewModel> platformTransferRecord = new PagedResultModel<TransferRecordViewModel>();

            List<TransferRecordOrderStatus> allTransferRecordOrderStatus = TransferRecordOrderStatus.GetAll();
            TransferRecordOrderStatus transferRecordOrderStatus = null;

            if (param.OrderStatus.HasValue)
            {
                transferRecordOrderStatus = TransferRecordOrderStatus.GetSingle(param.OrderStatus.Value);
            }

            foreach (SearchTransferType transferType in searchTransferTypes)
            {
                if (transferType == SearchTransferType.In)
                {
                    if (transferRecordOrderStatus != null)
                    {
                        queryPlatformParam.DealType = transferRecordOrderStatus.CorrelationMoneyInDealType.Value;
                    }

                    var cmoneyInInfoRep = ResolveJxBackendService<ICMoneyInInfoRep>().Value;
                    platformTransferRecord = cmoneyInInfoRep.GetPlatformTransferRecord(queryPlatformParam);

                    platformTransferRecord.ResultList.ForEach(r =>
                    {
                        r.TransferSource = _miseName;
                        r.TransferTarget = _platformName;
                        r.TransferType = transferType.Value;
                        r.StatusText = allTransferRecordOrderStatus
                            .Single(s => s.CorrelationMoneyInDealType == MoneyInDealType.GetSingle(r.Status))
                            .Name; ;
                    });
                }
                else if (transferType == SearchTransferType.Out)
                {
                    if (transferRecordOrderStatus != null)
                    {
                        queryPlatformParam.DealType = transferRecordOrderStatus.CorrelationMoneyOutDealType.Value;
                    }

                    var cmoneyOutInfoRep = ResolveJxBackendService<ICMoneyOutInfoRep>().Value;
                    platformTransferRecord = cmoneyOutInfoRep.GetPlatformTransferRecord(queryPlatformParam);

                    platformTransferRecord.ResultList.ForEach(r =>
                    {
                        r.TransferSource = _platformName;
                        r.TransferTarget = _miseName;
                        r.TransferType = transferType.Value;
                        r.StatusText = allTransferRecordOrderStatus
                            .Single(s => s.CorrelationMoneyOutDealType == MoneyOutDealType.GetSingle(r.Status))
                            .Name;
                    });
                }

                result = ConcatPagedResultModel(result, platformTransferRecord);
            }

            result.ResultList = result.ResultList
                .OrderByDescending(t => t.OrderTime)
                .ThenByDescending(t => t.OrderID)
                .Skip(queryTPParam.GetComputedOffset())
                .Take(queryTPParam.PageSize)
                .ToList();

            return result;
        }

        private PagedResultModel<T> ConcatPagedResultModel<T>(PagedResultModel<T> source, PagedResultModel<T> target)
        {
            source.TotalCount += target.TotalCount;
            source.ResultList = source.ResultList.Concat(target.ResultList).ToList();

            return source;
        }
    }
}