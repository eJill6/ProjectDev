using FakeMSSeal.Models;

namespace FakeMSSeal.Mock
{
    public interface IBalanceMockServcie
    {
        BalanceResult AddBalance(int userId, decimal amount);

        BalanceResult GetBalance(int userId);

        BalanceResult SubstractBalance(int userId, decimal amount);
    }
}