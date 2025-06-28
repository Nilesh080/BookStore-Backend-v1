using System.Data;
using BookStore.API.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookStore.API.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BookStoreDB");
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<List<BookResponse>> GetAllAsync()
        {
            var query = "SELECT Id, Title, Description FROM Books";
            using (var connection = CreateConnection())
            {
                connection.Open();
                var books = await connection.QueryAsync<BookResponse>(query);
                return books.ToList();
            }
        }
        public async Task<BookResponse> GetByIdAsync(int id)
        {
            var query = "SELECT Id, Title, Description FROM Books WHERE ID = @Id";
            using (var connection = CreateConnection())
            {
                connection.Open();
                var book = await connection.QueryFirstOrDefaultAsync<BookResponse>(query, new {ID = id});
                return book;
            }
        }
        public async Task<int> Add(BookRequest request)
        {
            var query = @"
                INSERT INTO Books (Title, Description) 
                VALUES (@Title, @Description);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            using (var connection = CreateConnection())
            {
                connection.Open();
                return await connection.QuerySingleAsync<int>(query, request);
            }
        }
        public async Task Update(int id, BookRequest request)
        {
            var query = "UPDATE Books SET Title = @Title, Description = @Description WHERE ID = @ID";

            var parameters = new
            {
                ID = id,
                request.Title,
                Description = request.Description
            };

            using (var connection = CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<bool> Delete(int id)
        {
            var query = "DELETE FROM Books WHERE ID = @ID";
            using (var connection = CreateConnection())
            {
                connection.Open();
                var result = await connection.ExecuteAsync(query, new { Id = id });
                return result > 0;
            }
        }
    }
}