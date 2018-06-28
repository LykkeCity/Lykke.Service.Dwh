using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using Lykke.Service.Dwh.Core.Services;

namespace Lykke.Service.Dwh.Services
{
    public class SqlAdapter : ISqlAdapter
    {
        private const string _spNameParam = "spname";
        private const string _formatParam = "format";

        private readonly string _connectionString;

        public string StoredProcedureParamName => _spNameParam;

        public SqlAdapter(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CallStoredProcedureAndFillDataSet(Dictionary<string, StringValues> parameters, DataSet dataSet)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = parameters[_spNameParam];
                command.CommandType = CommandType.StoredProcedure;
                foreach (var param in parameters)
                {
                    if (param.Key != _spNameParam && param.Key != _formatParam)
                        command.Parameters.AddWithValue("@" + param.Key, param.Value.First());
                }
                SqlDataAdapter theDataAdapter = new SqlDataAdapter(command);
                theDataAdapter.Fill(dataSet);
            }
        }
    }
}
