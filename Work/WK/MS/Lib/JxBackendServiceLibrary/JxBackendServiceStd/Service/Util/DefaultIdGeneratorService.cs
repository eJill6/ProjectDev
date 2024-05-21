using IdGen;
using JxBackendService.Interface.Repository.Util;
using JxBackendService.Interface.Service.Chat;
using JxBackendService.Model.Enums;
using JxBackendService.Model.ViewModel;
using JxBackendService.Repository.Util;
using JxBackendService.Service.Base;
using System;

namespace JxBackendService.Service.Util
{
    public class DefaultIdGeneratorService : BaseService, IIdGeneratorService
    {
        private static IdGenerator s_IdGenerator;

        private static readonly object s_lock = new object();

        private readonly Lazy<IMachineGeneratorRep> _machineGeneratorRep;

        public DefaultIdGeneratorService(EnvironmentUser envLoginUser, DbConnectionTypes dbConnectionType) : base(envLoginUser, dbConnectionType)
        {
            _machineGeneratorRep = ResolveJxBackendService<IMachineGeneratorRep>();

            if (s_IdGenerator == null)
            {
                lock (s_lock)
                {
                    int generatorId = GetOrCreateGeneratorId(Environment.MachineName);
                    s_IdGenerator = new IdGenerator(generatorId);
                }
            }
        }

        public long CreateId() => s_IdGenerator.CreateId();

        public bool TryCreateId(out long id) => s_IdGenerator.TryCreateId(out id);

        public int GetOrCreateGeneratorId(string machineName)
        {
            return _machineGeneratorRep.Value.GetOrCreateGeneratorId(machineName, 1023);
        }
    }
}