using System.Collections.Generic;
using Refit;

namespace Lykke.Service.Dwh.Client
{
    public interface IDwhClient
    {
        [Post("/api/StoredProcedures")]
        ResponceDataSet Get();

        [Post("/api/StoredProcedures/call/{procname}")]
        ResponceDataSet Call(Dictionary<string, string> parameters, string procname);

        [Post("/api/StoredProcedures/call/{procname}/{database}")]
        ResponceDataSet Call(Dictionary<string, string> parameters, string procname, string database);
    }
}
