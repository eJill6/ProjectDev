using System.Linq.Expressions;

namespace MS.Core.Infrastructures.DBTools
{
    public class OrderByExpressionComponent<T>
    {
        public OrderByExpressionComponent(Expression<Func<T, object>> orderPredicates, bool isDesc = false)
        {
            OrderPredicates = orderPredicates;
            IsDesc = isDesc;
        }

        public Expression<Func<T, object>> OrderPredicates { get; set; }
        public bool IsDesc { get; set; }
    }
}
