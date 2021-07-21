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
    public class Database
    {
        private readonly SqlConnection _db;
        //private IEnumerable<string> _data;

        public Database()
        {
            _db = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=MyInstagram;Trusted_Connection=True;MultipleActiveResultSets=true");
            _db.Open();
        }


        public Task<List<T>> GetAllItem<T>(string TableName)
        {
            string QueryString = $"SELECT * FROM dbo.{TableName}";

            var result = ExecuteCommand<List<T>>(_db, QueryString, null);


            return result;
        }

        private Task<T> ExecuteCommand<T>(SqlConnection connection, string queryString, string[] parameter)
        {
            Type itemType = typeof(T).GetGenericArguments()[0];

            StringBuilder cols = new StringBuilder();

            try
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                using (SqlDataAdapter data = new SqlDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    data.Fill(table);

                    PropertyInfo[] properties = itemType.GetProperties();
                    T result = default(T);

                    DataRowCollection row = table.Rows;
                    DataColumnCollection col = table.Columns;

                    cols.Append("[\n");

                    for (int x = 0; x < row.Count; x++)
                    {
                        //Console.WriteLine(row[x].ToString());
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

                    result = JsonSerializer.Deserialize<T>(cols.ToString());

                    return Task.FromResult(default(T));
                }
            }
            catch (JsonException jx)
            {
                Console.WriteLine(jx.Message);
                return Task.FromResult(default(T));
            }
        }


        public void Dispose()
        {
            _db.Close();
        }

    }
}
