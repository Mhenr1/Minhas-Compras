using Minhas_Compras.Models;
using SQLite;

namespace Minhas_Compras.Helpers
{
    public class SQLiteDatabaseHelpers
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelpers(string path)
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
            string sql = "UPDATE Produto SET Descricao= ?, Preco = ?, Quantidade = ? WHERE Id = ?";
            return _conn.QueryAsync<Produto>(sql,
                p.Descricao,
                p.Preco,
                p.Quantidade,
                p.Id);
        }
        public Task<int> Delete(int id)
        {
           return _conn.Table<Produto>().DeleteAsync(i => i.Id  == id);
        }
        public Task<List<Produto>> GetAll()
        {
           return _conn.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string q)
        {
            string sql = "Select * from Produto where Descricao like '%" + q + "%'";
            return _conn.QueryAsync<Produto>(sql);
        }
    }

}