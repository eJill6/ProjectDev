using JxBackendService.Model.Enums;
using JxBackendService.Model.ReturnModel;
using System.Collections.Generic;
using System.Data;

namespace JxBackendService.Interface.Repository
{
    public interface IBaseDbRepository<T>
    {
        BaseReturnDataModel<long> CreateByProcedure(InlodbType inlodbType, T model);

        BaseReturnDataModel<long> CreateByProcedure(T model);

        BaseReturnDataModel<long> CreateByProcedure(T model, string tableName);

        bool CreateListByProcedure(IList<T> models);

        bool DeleteByProcedure(InlodbType inlodbType, T model);

        bool DeleteByProcedure(T model);

        bool DeleteListByProcedure(IList<T> models);

        T GetSingleByKey(InlodbType inlodbType, T model);

        T GetSingleByKey(InlodbType inlodbType, T model, bool isClearUpdateUserAndTime);

        //string GetTableSequence();

        bool UpdateByProcedure(InlodbType inlodbType, T model);

        bool UpdateByProcedure(T model);

        bool UpdateByProcedure(T model, bool isSystemJob);

        bool UpdateListByProcedure(IList<T> models);

        DataTable CreateEmptyDataTable(InlodbType inlodbType);

        void BulkCopy(InlodbType inlodbType, DataTable dataTable);
    }
}