using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace Lykke.Service.Dwh.Client
{
    /// <summary>
    /// Dwh service client interface.
    /// </summary>
    public interface IDwhClient
    {
        /// <summary>
        /// Calls stored procedure with params from query.
        /// </summary>
        /// <returns>stored procedure execution result.</returns>
        [Post("/api/StoredProcedures")]
        Task<ResponceDataSet> Get();

        /// <summary>
        /// Calls stored procedure with provided name and params on default database.
        /// </summary>
        /// <param name="parameters">Stored procedure key-value parameters dictionary.</param>
        /// <param name="procname">Name of stored procedure to be executed.</param>
        /// <returns>stored procedure execution result.</returns>
        [Post("/api/StoredProcedures/call/{procname}")]
        Task<ResponceDataSet> Call(Dictionary<string, string> parameters, string procname);

        /// <summary>
        /// Calls stored procedure with provided name and params on specified database.
        /// </summary>
        /// <param name="parameters">Stored procedure key-value parameters dictionary.</param>
        /// <param name="procname">Name of stored procedure to be executed.</param>
        /// <param name="database">Database to work with.</param>
        /// <returns>stored procedure execution result.</returns>
        [Post("/api/StoredProcedures/call/{procname}/{database}")]
        Task<ResponceDataSet> Call(Dictionary<string, string> parameters, string procname, string database);
    }
}
