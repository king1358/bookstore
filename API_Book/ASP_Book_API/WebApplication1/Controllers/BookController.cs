using Dapper;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        
        [HttpGet]
        public List<Book> GetAllBook()
        {
            
            using (var conn = new SqlConnection("Data Source=LAPTOP-4N02CR39;Initial Catalog=BOOKSTORE;User ID=sa1;Password=svcntt;TrustServerCertificate=True"))
            {
                conn.Open();
                string sql = "Select * from BOOK";
                List<Book> list = conn.Query<Book>(sql).ToList();
                return list;
            }
            
        }
        [HttpGet("id")]
        public Book GetByID(string id)
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-4N02CR39;Initial Catalog=BOOKSTORE;User ID=sa1;Password=svcntt;TrustServerCertificate=True"))
            {
                conn.Open();
                string sql = "Select * from BOOK where Id = @id";
                Book b = conn.QueryFirstOrDefault<Book>(sql,new {id = id});
                return b;
            }
        }


    }
}
