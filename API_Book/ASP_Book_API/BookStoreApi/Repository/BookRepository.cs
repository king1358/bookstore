using BookStoreApi.Interface;
using BookStoreApi.Model;
using BookStoreApi.Service;
using System.Collections;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStoreApi.Repository
{
    public class BookRepository : IBook
    {
        private readonly IService _service;
        public BookRepository(IService service)
        {
            _service = service;
        }
        public async Task<List<Book>> GetAllBook()
        {

            return await _service.GetAll<Book>("SELECT * FROM BOOK", new { });
     
        }
        public async Task<Book> GetBook(int id)
        {
            return await _service.GetAsync<Book>("SELECT * FROM BOOK WHERE ID = @ID", new { ID = id });
        }
    }
}
