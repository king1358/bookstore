using BookStoreApi.Model;

namespace BookStoreApi.Interface
{
    public interface IBook
    {
        Task<List<Book>> GetAllBook();
        Task<Book> GetBook(int id);

    }
}
