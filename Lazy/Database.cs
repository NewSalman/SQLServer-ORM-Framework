using System.Reflection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;
using System.Buffers.Text;
using System.Buffers;

namespace Database.Lazy
{
    public class Database<T> : IDatabase<T>
    {
        private SqlConnection _db;

        public string ConnectionStrings { get; set; }

        private List<T> _data { get; set; }
        public Database()
        {

        }

        public void Init()
        {
            _db = new SqlConnection(ConnectionStrings);
            _db.Open();
        }


        public Task<List<T>> GetAllItem(string TableName)
        {
            string QueryString = $"SELECT * FROM dbo.{TableName}";

            var result = ExecuteCommand(_db, QueryString, null);

            Task.Run(async () =>
            {
                _data = await ExecuteCommand(_db, QueryString, null);
            });
            return result;
        }

        private Task<List<T>> ExecuteCommand(SqlConnection connection, string queryString, string[] parameter)
        {

            StringBuilder cols = new StringBuilder();

            try
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                using (SqlDataAdapter data = new SqlDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    data.Fill(table);

                    List<T> result = default(List<T>);

                    DataRowCollection row = table.Rows;
                    DataColumnCollection col = table.Columns;

                    cols.Append("[\n");

                    for (int x = 0; x < row.Count; x++)
                    {
                        cols.Append("{\n");
                        for (int i = 0; i < col.Count; i++)
                        {
                            if (col[i] == col[col.Count - 1])
                            {
                                if (row[x][col[i]].GetType() == typeof(string) || row[x][col[i]].GetType() == typeof(DateTime))
                                {
                                    cols.Append(string.Format(" \"{0}\" : \"{1}\" \n", col[i].ColumnName, row[x][col[i]]));
                                }
                                else
                                {
                                    cols.Append(string.Format(" \"{0}\" : {1} \n", col[i].ColumnName, row[x][col[i]]));
                                }
                                break;
                            }
                            if (row[x][col[i]].GetType() == typeof(string) || row[x][col[i]].GetType() == typeof(DateTime))
                            {
                                cols.Append(string.Format(" \"{0}\" : \"{1}\", \n", col[i].ColumnName, row[x][col[i]]));
                            }
                            else
                            {
                                cols.Append(string.Format(" \"{0}\" : {1}, \n", col[i].ColumnName, row[x][col[i]]));
                            }

                        }

                        if (row[x] == row[row.Count - 1])
                        {
                            cols.Append("}\n".Trim(','));
                            break;
                        }
                        cols.Append("},\n");
                    }
                    cols.Append("]");

                    result = JsonSerializer.Deserialize<List<T>>(cols.ToString());

                    return Task.FromResult(result);
                }
            }
            catch (JsonException jx)
            {
                Console.WriteLine(jx.Message);
                return Task.FromResult(default(List<T>));
            }
        }


        public void Dispose()
        {
            _db.Close();
        }

    }
}
