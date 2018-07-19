using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Lykke.Service.Dwh.Client
{
    public interface IDwhClient
    {
        [Post("/api/StoredProcedures")]
        Task<ResponceDataSet> Get();

        [Post("/api/StoredProcedures/call/{procname}")]
        Task<ResponceDataSet> Call(Dictionary<string, string> parameters, string procname);

        [Post("/api/StoredProcedures/call/{procname}/{database}")]
        Task<ResponceDataSet> Call(Dictionary<string, string> parameters, string procname, string database);
    }
}
