using System.Data;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace Lykke.Service.Dwh.Core.Services
{
    public interface ISqlAdapter
    {
        string StoredProcedureParamName { get; }

        void CallStoredProcedureAndFillDataSet(Dictionary<string, StringValues> parameters, DataSet dataSet);
    }
}
