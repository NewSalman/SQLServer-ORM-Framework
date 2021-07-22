using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lazy
{
    public interface IDatabase<T> : IDisposable
    {
        Task Init();
        Task<List<T>> GetAllItem();
        Task<T> GetItemByID(string id);
        Task<List<T>> GetItemBy(string parameter, string ColumnName);
        Task UpdateItem(T ItemToUpdate);
        Task<T> DeleteItem(string ColumnName, string parameter);
    }
}
