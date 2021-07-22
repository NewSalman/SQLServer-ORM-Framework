using System.Xml.Linq;
using System.ComponentModel;
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

        private string ConnectionStrings { get; set; }

        private string TableName { get; set; }

        private StringBuilder _data { get; set; }

        private IList<T> Result { get; set; }

        public Database(string tableName, string connectionStrings)
        {
            this.ConnectionStrings = connectionStrings;
            this.TableName = tableName;
        }


        public async Task Init()
        {

            _db = new SqlConnection(ConnectionStrings);
            _db.Open();

            Result = await ExecuteCommand(_db, $"SELECT * FROM dbo.{TableName}", null);

        }


        public Task<List<T>> GetAllItem()
        {
            return Task.FromResult(Result.ToList());
        }

        public Task<T> GetItemByID(string id)
        {
            T resultItem = default(T);

            for (int i = 0; i < Result.Count; i++)
            {
                string itemProperty = GetPropertyValue(Result[i], "ID");
                if (itemProperty == id)
                {
                    resultItem = Result[i];
                    break;
                }
            }

            return Task.FromResult(resultItem);
        }

        public Task<List<T>> GetItemBy(string parameter, string By)
        {
            List<T> resultItem = new List<T>();

            for (int i = 0; i < Result.Count; i++)
            {
                string itemProperty = GetPropertyValue(Result[i], By);
                if(itemProperty.ToLower() == parameter.ToLower()) {
                    Console.WriteLine(itemProperty);
                    resultItem.Add(Result[i]);
                }

            }
            return Task.FromResult(resultItem);
        }
        public Task UpdateItem(T ItemToUpdate)
        {
            // Type item = ItemToUpdate.GetType();

            // Task[] tasks = new Task[] { };

            // Task OnList = new Task(() =>
            // {
            //     T itemToUpdate = _data.Where( != id)
            //     if (itemToUpdate == null)
            //     {
            //         throw new ArgumentNullException("item is null");
            //     }

            //     Console.WriteLine(ItemId);

            // });

            // Task OnDB = new Task(() =>
            // {

            // });

            // tasks.Append(OnList);
            // tasks.Append(OnDB);

            // await Task.WhenAll(tasks);

            throw new NotImplementedException();

        }
        public Task<T> DeleteItem(string ColumnName, string parameter)
        {
            throw new NotImplementedException();
        }
        public Task<List<T>> GetItemBy(string ColumnName, string[] paramter)
        {
            throw new NotImplementedException();
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

                    _data = cols;

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

        private string GetPropertyValue(object obj, string PropertyName)
        {
            Type type = obj.GetType();

            return type.GetProperty(PropertyName).GetValue(obj).ToString();
        }


        public void Dispose()
        {
            //Result.Clear();
            _db.Close();
        }

    }
}
