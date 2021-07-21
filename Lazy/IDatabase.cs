using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Database.Lazy
{
    public interface IDatabase : IDisposable
    {

        Task<IEnumerable<string>> GetAllItem(string TableName);

    }
}
