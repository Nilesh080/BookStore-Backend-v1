using BookStore.API.DTOs;

namespace BookStore.API.Repository
{
    public interface IBookRepository
    {
        Task<List<BookResponse>> GetAllAsync();
        Task<BookResponse> GetByIdAsync(int id);
        Task<int> Add(BookRequest bookModel);
        Task Update(int id, BookRequest bookModel);
        Task<bool> Delete(int id);
    }
}
