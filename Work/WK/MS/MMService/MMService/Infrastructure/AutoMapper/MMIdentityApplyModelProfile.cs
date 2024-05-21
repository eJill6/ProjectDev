using Amazon.Auth.AccessControlPolicy;
using AutoMapper;
using MS.Core.MM.Models.Auth;
using MS.Core.MM.Models.Auth.ServiceReq;
using MS.Core.MM.Models.Entities.PostTransaction;
using MS.Core.MM.Models.Entities.User;
using MS.Core.MMModel.Models.AdminIncomeExpense;
using MS.Core.MMModel.Models.AdminPostTransaction;
using MS.Core.MMModel.Models.AdminUserManager;
using MS.Core.MMModel.Models.Post;
using MS.Core.MMModel.Models.Post.Enums;
using MS.Core.MMModel.Models.User;
using MS.Core.MMModel.Models.User.Enums;
using NLog.Targets;

namespace MMService.Infrastructure.AutoMapper
{
    public class MMIdentityApplyModelProfile : Profile
    {
       
   

        /// <summary>
        /// 商家照片
        /// </summary>
        public string[] ShopPhotoIds { get; set; } = new string[0];
        public MMIdentityApplyModelProfile()
        {
            CreateMap<MMIdentityApply, AdminUserManagerIdentityApplyList>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => (ReviewStatus)src.Status))
				.ForMember(dest => dest.OriginalIdentity, opt => opt.MapFrom(src => (IdentityType)src.OriginalIdentity))
				.ForMember(dest => dest.ApplyIdentity, opt => opt.MapFrom(src => (IdentityType)src.ApplyIdentity));

            CreateMap<BossIdentityApplyData, ReqBossIdentityApplyData>();
            CreateMap<AgentIdentityApplyData, ReqAgentIdentityApplyData>();
            CreateMap<MMIdentityApply, IdentityApplyForClient>();

            CreateMap<OfficialShopDetailForclient, ReqBossApplyOrUpdateData>();
            
        }
    }
}
