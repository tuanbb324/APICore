using Dapper;
using System.Data;
using System.Data.Common;

namespace ACBSChatbotConnector.Repositories
{
    public interface IDapperDA
    {
        void Dispose();
        Task ExecuteAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        Task<object> ExecuteScalarAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
        Task<IEnumerable<T>> GetListAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<IEnumerable<T>> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
        DbConnection GetDbconnection();
        Task<IEnumerable<T>> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<IEnumerable<T>> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }
}