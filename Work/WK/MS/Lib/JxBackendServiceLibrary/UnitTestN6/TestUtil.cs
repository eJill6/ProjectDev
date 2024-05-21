using IdGen;
using JxBackendService.Common.Util;
using JxBackendService.Model.Common;
using JxBackendService.Model.Enums;
using System;

namespace UnitTestN6
{
    public static class TestUtil
    {
        private static readonly IdGenerator s_idGenerator = new IdGenerator(0);

        public static long CreateId()
        {
            while (true)
            {
                if (s_idGenerator.TryCreateId(out long id))
                {
                    return id;
                }

                TaskUtil.DelayAndWait(1000);
            }
        }

        public static void DoPlayerJobs(int recordCount, Action<string, long> job)
        {
            string accountPrefixCode = SharedAppSettings.GetEnvironmentCode().AccountPrefixCode;

            string[] playerIds = new string[]
            {
                $"jx{accountPrefixCode}_69778",
                $"cts{accountPrefixCode}_3",
                $"cts{accountPrefixCode}_36",
                $"msl{accountPrefixCode}_588",
                $"msl{accountPrefixCode}_888",
            };

            DoPlayerJobs(playerIds, recordCount, job);
        }

        public static void DoPlayerJobs(string[] playerIds, int recordCount, Action<string, long> job)
        {
            foreach (string playerId in playerIds)
            {
                for (int i = 0; i < recordCount; i++)
                {
                    long id = CreateId();
                    job.Invoke(playerId, id);
                }
            }
        }
    }
}