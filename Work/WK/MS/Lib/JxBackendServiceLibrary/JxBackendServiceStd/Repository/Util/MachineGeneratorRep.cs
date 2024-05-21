using JxBackendService.Common.Extensions;
using JxBackendService.Interface.Repository.Util;
using JxBackendService.Model.Entity.Util;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Base;
using System;
using System.Collections.Generic;

namespace JxBackendService.Repository.Util
{
    public class MachineGeneratorRep : BaseDbRepository<MachineGenerator>, IMachineGeneratorRep
    {

        public MachineGeneratorRep(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
        }

        public int GetOrCreateGeneratorId(string machineName, int maxGeneratorId)
        {
            int? generatorId = GetGeneratorId(machineName);

            if (generatorId.HasValue)
            {
                return generatorId.Value;
            }

            int availableGeneratorId = GetAvailableGeneratorId(maxGeneratorId);

            CreateByProcedure(
                InlodbType.Inlodb,
                new MachineGenerator()
                {
                    MachineName = machineName,
                    GeneratorID = availableGeneratorId,
                });

            return availableGeneratorId;
        }

        private int? GetGeneratorId(string machineName)
        {
            MachineGenerator machineGenerator = GetSingleByKey(InlodbType.Inlodb, new MachineGenerator() { MachineName = machineName });

            if (machineGenerator == null)
            {
                return null;
            }

            return machineGenerator.GeneratorID;
        }

        private int GetAvailableGeneratorId(int maxGeneratorId)
        {
            string sql = GetAllQuerySQL(InlodbType.Inlodb, new List<string>() { nameof(MachineGenerator.GeneratorID) });
            HashSet<int> generatorIdSet = DbHelper.QueryList<int>(sql, param: null).ConvertToHashSet();

            for (int i = 0; i <= maxGeneratorId ; i++){

                if (!generatorIdSet.Contains(i))
                {
                    return i;
                }
            }

            throw new ArgumentOutOfRangeException("no available GeneratorId");
        }
    }
}