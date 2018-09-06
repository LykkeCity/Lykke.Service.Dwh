using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.Api.Contract.Responses;
using Lykke.Common.ApiLibrary.Exceptions;
using Lykke.Common.Log;
using Lykke.Service.Dwh.Client;
using Lykke.Service.Dwh.Core.Services;
using Lykke.Service.Dwh.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Dwh.Controllers
{
    [Route("api/[controller]")]
    public class StoredProceduresController : Controller, IDwhClient
    {
        private readonly ISqlAdapter _sqlAdapter;
        private ILog _log;

        public StoredProceduresController(ISqlAdapter sqlAdapter, ILogFactory logFactory)
        {
            _sqlAdapter = sqlAdapter;
            _log = logFactory.CreateLog(this);
        }

        /// <summary>
        /// Get Method to Call the Stored Procedure from Configured Database
        /// </summary>
        /// <returns>Result dataSet</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponceDataSet), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public Task<ResponceDataSet> Get()
        {
            var paramsDict = QueryHelpers.ParseQuery(HttpContext.Request.Query.ToString());

            return Process(paramsDict);
        }

        /// <summary>
        /// Method Call the Stored Procedure from default Database (dwh)
        /// </summary>
        /// <param name="parameters">Dictionary with key-value pairs - parameters</param>
        /// <param name="procname">stored procedure name</param>
        /// <returns></returns>
        [SwaggerOperation("Call")]
        [HttpPost("call/{procname}")]
        [ProducesResponseType(typeof(ResponceDataSet), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public Task<ResponceDataSet> Call([FromBody] Dictionary<string, string> parameters, string procname)
        {
            parameters[SqlAdapter.SpNameParam] = procname;
            var result =  Process(parameters.ToDictionary(i => i.Key, i => new StringValues(i.Value)));
            return result;
        }

        /// <summary>
        /// Method Call the Stored Procedure from selected Database
        /// </summary>
        /// <param name="parameters">Dictionary with key-value pairs - parameters</param>
        /// <param name="procname">stored procedure name</param>
        /// <param name="database">database name (name from service config)</param>
        /// <returns></returns>
        [SwaggerOperation("Call")]
        [HttpPost("call/{procname}/{database}")]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public Task<ResponceDataSet> Call([FromBody] Dictionary<string, string> parameters, string procname, string database)
        {
            parameters[SqlAdapter.SpNameParam] = procname;
            parameters[SqlAdapter.DatabaseParam] = database;
            return Process(parameters.ToDictionary(i => i.Key, i => new StringValues(i.Value)));
        }

        private Task<ResponceDataSet> Process(Dictionary<string, StringValues> parameters)
        {
            try
            {
                if (!parameters.ContainsKey(_sqlAdapter.StoredProcedureParamName)
                || string.IsNullOrEmpty(parameters[_sqlAdapter.StoredProcedureParamName]))
                throw new ArgumentException("spname parameter is missing or empty");

                ResponceDataSet responce = new ResponceDataSet
                {
                    Data = new DataSet(),
                };

                _sqlAdapter.CallStoredProcedureAndFillDataSet(parameters, responce.Data);

                return Task.FromResult(responce);
            }
            catch (Exception ex)
            {
                _log.Info(nameof(Process), "Exception on execute request", exception: ex, context: $"params: {parameters.ToJson()}");
                throw new ValidationApiException(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}
