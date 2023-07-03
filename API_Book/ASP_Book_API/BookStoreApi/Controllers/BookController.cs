using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Service;
using BookStoreApi.Model;
using BookStoreApi.Interface;
namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBook _bookRepository;
        public BookController(IBook bookRepository) {
            _bookRepository = bookRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Book> book = await _bookRepository.GetAllBook();
                return Ok(book);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Book book = await _bookRepository.GetBook(id);
                if (book == null) return Ok("Not Found");
                return Ok(book);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
