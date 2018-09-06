using System;
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
        public const string SpNameParam = "spname";
        public const string FormatParam = "format";
        public const string DatabaseParam = "database";

        public const string DefaultDataBase = "dwh";

        private readonly IReadOnlyDictionary<string, string> _connectionString;

        public string StoredProcedureParamName => SpNameParam;

        public SqlAdapter(IReadOnlyDictionary<string, string> connectionStrings)
        {
            _connectionString = connectionStrings;
        }

        public void CallStoredProcedureAndFillDataSet(Dictionary<string, StringValues> parameters, DataSet dataSet)
        {

            if (!parameters.TryGetValue(DatabaseParam, out var databaseParam))
            {
                databaseParam = DefaultDataBase;
            }

            var database = databaseParam.ToString();

            using (var connection = new SqlConnection(_connectionString[database]))
            using (var command = connection.CreateCommand())
            {
                command.CommandTimeout = 600;
                connection.Open();
                command.CommandText = parameters[SpNameParam];
                command.CommandType = CommandType.StoredProcedure;
                foreach (var param in parameters)
                {
                    if (param.Key == SpNameParam || param.Key == FormatParam || param.Key == DatabaseParam)
                        continue;

                    var paramName = param.Key.StartsWith('@')
                        ? param.Key
                        : "@" + param.Key;

                    if (param.Value.Count == 0)
                        command.Parameters.Add(
                            new SqlParameter(paramName, DBNull.Value)
                            {
                                IsNullable = true,
                                Direction = ParameterDirection.Input,
                            });
                    else
                        command.Parameters.AddWithValue(paramName, param.Value.First());
                }
                SqlDataAdapter theDataAdapter = new SqlDataAdapter(command);
                theDataAdapter.Fill(dataSet);
            }
        }
    }
}
