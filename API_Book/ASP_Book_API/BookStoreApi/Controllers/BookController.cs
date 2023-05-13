using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Service;
using BookStoreApi.Model;
namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IService _service;
        public BookController(IService dbService) {
            _service = dbService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Book> book = await _service.GetAll<Book>("SELECT * FROM BOOK",new {});
                return Ok(book);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                Book book = await _service.GetAsync<Book>("SELECT * FROM BOOK WHERE ID = @ID", new {  ID = id });
                if (book == null) return Ok("Not Found");
                return Ok(book);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
