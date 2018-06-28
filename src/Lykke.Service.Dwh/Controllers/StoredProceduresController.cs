using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.SwaggerGen;
using Lykke.Service.Dwh.Client;
using Lykke.Service.Dwh.Core.Services;

namespace Lykke.Service.Dwh.Controllers
{
    [Route("api/[controller]")]
    public class StoredProceduresController : Controller, IDwhClient
    {
        private readonly ISqlAdapter _sqlAdapter;

        public StoredProceduresController(ISqlAdapter sqlAdapter)
        {
            _sqlAdapter = sqlAdapter;
        }

        /// <summary>
        /// Get Method to Call the Stored Procedure from Configured Database
        /// </summary>
        /// <param name="parameters">Dictionary with key-value pairs could contain at list spname parameter with stored procedure name</param>
        /// <returns>Result dataSet</returns>
        [HttpPost]
        public ResponceDataSet Get()
        {
            var paramsDict = QueryHelpers.ParseQuery(HttpContext.Request.Query.ToString());

            return Process(paramsDict);
        }

        /// <summary>
        /// Method Call the Stored Procedure from Configured Database
        /// </summary>
        /// <param name="parameters">Dictionary with key-value pairs could contain at list spname parameter with stored procedure name</param>
        /// <returns></returns>
        [SwaggerOperation("Call")]
        [HttpPost("call")]
        public ResponceDataSet Call([FromBody] Dictionary<string, string> parameters)
        {
            return Process(parameters.ToDictionary(i => i.Key, i => new StringValues(i.Value)));
        }

        private ResponceDataSet Process(Dictionary<string, StringValues> parameters)
        {
            if (!parameters.ContainsKey(_sqlAdapter.StoredProcedureParamName)
                || string.IsNullOrEmpty(parameters[_sqlAdapter.StoredProcedureParamName]))
                throw new ArgumentException("spname parameter is missing or empty");

            ResponceDataSet responce = new ResponceDataSet
            {
                Data = new DataSet(),
            };

            _sqlAdapter.CallStoredProcedureAndFillDataSet(parameters, responce.Data);

            return responce;
        }
    }
}
