using System.Linq.Expressions;

namespace MS.Core.Infrastructures.DBTools
{
    public class ExpressionComponent<T>
    {
        private List<Expression<Func<T, bool>>> WhereExpressions { get; set; } = new List<Expression<Func<T, bool>>>();

        private Expression<Func<T, object>> Predicate { get; set; }

        private List<OrderByExpressionComponent<T>> OrderPredicates { get; set; } = new List<OrderByExpressionComponent<T>> { };
        

        public ExpressionComponent<T> Where(Expression<Func<T, bool>> predicate)
        {
            WhereExpressions.Add(predicate);
            return this;
        }

        public ExpressionComponent<T> Select<S>(Expression<Func<T, S>> predicate)
        {
            Predicate = Expression.Lambda<Func<T, object>>(predicate.Body, predicate.Parameters);
            return this;
        }

        public ExpressionComponent<T> OrderBy(Expression<Func<T, object>> predicate)
        {
            OrderPredicates.Add(new OrderByExpressionComponent<T>(predicate));
            return this;
        }

        public ExpressionComponent<T> OrderByDescending(Expression<Func<T, object>> predicate)
        {
            OrderPredicates.Add(new OrderByExpressionComponent<T>(predicate, true));
            return this;
        }

        public ExpressionComponent<T> Sum(Expression<Func<T, object>> predicate)
        {
            Predicate = predicate;
            return this;
        }

        public List<Expression<Func<T, bool>>> GetWhereExpressions()
        {
            return WhereExpressions;
        }

        public Expression<Func<T, object>> GetPredicate()
        {
            return Predicate;
        }

        public List<OrderByExpressionComponent<T>> GetOrderByPredicate()
        {
            return OrderPredicates;
        }
    }
}
