using Dapper;
using MS.Core.Infrastructures.Exceptions;
using MS.Core.Models;
using MS.Core.Models.Models;
using System.Data;
using System.Linq.Expressions;

namespace MS.Core.Infrastructures.DBTools
{
    public class DapperQueryComponent<T> where T : class
    {
        private DapperComponent DapperComponent { get; }
        private ExpressionComponent<T> ExpressionComponent { get; }

        public DapperQueryComponent(DapperComponent dapperComponent)
        {
            DapperComponent = dapperComponent;
            ExpressionComponent = new ExpressionComponent<T> { };
        }

        public DapperQueryComponent<T> Where(Expression<Func<T, bool>> predicate)
        {
            ExpressionComponent.Where(predicate);
            return this;
        }

        public DapperQueryComponent<T> OrderBy(Expression<Func<T, object>> predicate)
        {
            ExpressionComponent.OrderBy(predicate);
            return this;
        }

        public DapperQueryComponent<T> OrderByDescending(Expression<Func<T, object>> predicate)
        {
            ExpressionComponent.OrderByDescending(predicate);
            return this;
        }

        public async Task<PageResultModel<T>> QueryPageResultAsync(PaginationModel page)
        {
            if (page.PageSize == 0)
            {
                throw new MSException(ReturnCode.ParameterIsInvalid);
            }
            var parameters = new DynamicParameters();
            parameters.Add(SQLQueryUtil<T>.TotalCount, dbType: DbType.Int32, direction: ParameterDirection.Output);

            string sql = SQLQueryUtil<T>.BuildPageSql(parameters, ExpressionComponent, page);
            var result = await DapperComponent.QueryAsync<T>(sql, parameters);

            var totalCount = parameters.Get<int>(SQLQueryUtil<T>.TotalCount);

            var totalPage = (int)Math.Ceiling((decimal)totalCount / page.PageSize);

            return new PageResultModel<T>
            {
                PageNo = page.PageNo,
                PageSize = page.PageSize,
                TotalPage = totalPage,
                TotalCount = totalCount,
                Data = result.ToArray(),
            };
        }

        public async Task<IEnumerable<S>> SelectQueryAsync<S>(Expression<Func<T, S>> predicate) where S : class
        {
            ExpressionComponent.Select(predicate);
            var parameters = new DynamicParameters();
            string sql = SQLQueryUtil<T>.BuildSql(parameters, ExpressionComponent);
            return await DapperComponent.QueryAsync<S>(sql, parameters);
        }

        public async Task<S> SumAsync<S>(Expression<Func<T, object>> predicate)
        {
            ExpressionComponent.Sum(predicate);
            var parameters = new DynamicParameters();
            string sql = SQLQueryUtil<T>.BuildSumSql(parameters, ExpressionComponent);
            return await DapperComponent.QueryScalarAsync<S>(sql, parameters);
        }

        public async Task<IEnumerable<T>> QueryAsync()
        {
            var parameters = new DynamicParameters();
            string sql = SQLQueryUtil<T>.BuildSql(parameters, ExpressionComponent);
            return await DapperComponent.QueryAsync<T>(sql, parameters);
        }

        public async Task<T> QueryFirstAsync()
        {
            var parameters = new DynamicParameters();
            string sql = SQLQueryUtil<T>.BuildSql(parameters, ExpressionComponent);
            return await DapperComponent.QueryFirstAsync<T>(sql, parameters);
        }

        public async Task<int> CountAsync()
        {
            var parameters = new DynamicParameters();
            string sql = SQLQueryUtil<T>.BuildCountSql(parameters, ExpressionComponent);
            return await DapperComponent.QueryScalarAsync<int>(sql, parameters);
        }

        public async Task<bool> IsExistsAsync()
        {
            var parameters = new DynamicParameters();
            string sql = SQLQueryUtil<T>.BuildExistsSql(parameters, ExpressionComponent);
            return await DapperComponent.QueryScalarAsync<bool>(sql, parameters);
        }
    }
}