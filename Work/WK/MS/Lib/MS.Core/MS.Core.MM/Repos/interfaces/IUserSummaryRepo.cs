using MS.Core.Infrastructures.DBTools;
using MS.Core.MM.Models;
using MS.Core.MM.Models.Entities.User;

namespace MS.Core.MM.Repos.interfaces
{
    public interface IUserSummaryRepo
    {
        Task<decimal> GetUserAmount(int userId, UserSummaryTypeEnum type, UserSummaryCategoryEnum category);
        Task<IEnumerable<MMUserSummary>> GetUserSummaries(IEnumerable<int> userIds);
        Task IncrementUserQuantity(IncrementUserSummaryModel entity);
        DapperComponent IncrementUserSummary(DapperComponent writeDb, IncrementUserSummaryModel entity);
        DapperComponent IncrementUserSummary(DapperComponent writeDb, IEnumerable<IncrementUserSummaryModel> entities);
        Task RestSetAmount(UserSummaryTypeEnum type, decimal amount, DateTime time);
    }
}