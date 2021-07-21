using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lazy
{
    public interface IDatabase<T> : IDisposable
    {
        string ConnectionStrings { get; set; }

        void Init();

        Task<List<T>> GetAllItem(string TableName);

        Task<T> GetItem(string ColumnName, string parameter);
        Task<T> UpdateItem(string ColumnName, string parameter);
        Task<T> DeleteItem(string ColumnName, string parameter);

    }
}
