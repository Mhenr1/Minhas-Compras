using Minhas_Compras.Models;
using SQLite;

namespace Minhas_Compras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }
        public Task<List<Produto>> Update(Produto p)
        {
            _conn.UpdateAsync(p);
            return _conn.Table<Produto>().ToListAsync();

        }
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string busca, string? categoria)
        {
            string sql = "Select * from Produto where Descricao like '%" + busca + "%' ";
            if (!string.IsNullOrEmpty(categoria))
            {
              sql +=  " and Categoria = '" + categoria + "'";
            }
            return _conn.QueryAsync<Produto>(sql);
        }
    }

}