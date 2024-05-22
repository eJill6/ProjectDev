using FakeMSSeal.Models;
using System.Collections.Concurrent;

namespace FakeMSSeal.Mock
{
    public class MockBalanceServcie : IBalanceMockServcie
    {
        private static readonly decimal _defaultUserBalance = 1000m;

        private static readonly object _locker = new();

        private static readonly ConcurrentDictionary<int, decimal> s_userBalanceMap = new();

        public BalanceResult GetBalance(int userId)
        {
            decimal balance;

            lock (_locker)
            {
                balance = GetBalanceAmount(userId);
            }

            return new BalanceResult()
            {
                Success = true,
                Data = new BalanceDetail()
                {
                    Balance = balance.ToString()
                }
            };
        }

        public BalanceResult AddBalance(int userId, decimal amount)
        {
            decimal balance;

            lock (_locker)
            {
                balance = GetBalanceAmount(userId);
                balance += amount;
                s_userBalanceMap[userId] = balance;
            }

            return new BalanceResult()
            {
                Success = true,
                Data = new BalanceDetail()
                {
                    Balance = balance.ToString()
                }
            };
        }

        public BalanceResult SubstractBalance(int userId, decimal amount)
        {
            decimal balance;

            lock (_locker)
            {
                balance = GetBalanceAmount(userId);

                if (balance < amount)
                {
                    return new BalanceResult()
                    {
                        Success = false,
                        Error = "餘額不足"
                    };
                }

                balance -= amount;
                s_userBalanceMap[userId] = balance;
            }

            return new BalanceResult()
            {
                Success = true,
                Data = new BalanceDetail()
                {
                    Balance = balance.ToString()
                }
            };
        }

        private static decimal GetBalanceAmount(int userId)
        {
            decimal balance;

            lock (_locker)
            {
                if (!s_userBalanceMap.TryGetValue(userId, out balance))
                {
                    balance = _defaultUserBalance;
                    s_userBalanceMap.TryAdd(userId, balance);
                }
            }

            return balance;
        }
    }
}