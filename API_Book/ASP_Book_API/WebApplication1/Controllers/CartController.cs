using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class CartController : ControllerBase
    {


        [HttpGet]
        public ResultCart GetByUser(string username)
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-4N02CR39;Initial Catalog=BOOKSTORE;User ID=sa1;Password=svcntt;TrustServerCertificate=True"))
            {
                conn.Open();
                try
                {
                    string id = conn.QueryFirst<string>("SELECT ID FROM [USER] WHERE USERNAME = @USERNAME", new { username = username });

                    string sqlquery = "select ID_BOOK,QUANTITY FROM ITEMCART WHERE ID_USER = @USERNAME";
                    List<CartItem> cartUser = conn.Query<CartItem>(sqlquery, new { username = id, }).ToList();
                    //return OK(usr[0].dat);
                   
                    return new ResultCart{ result = "Success", items= cartUser };
                }
                catch
                {
                    return new ResultCart { result = "Fail", items = null };
                }
            }
        }


        [HttpPost]
        public string AddCart([FromBody] CartItem cart)
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-4N02CR39;Initial Catalog=BOOKSTORE;User ID=sa1;Password=svcntt;TrustServerCertificate=True"))
            {
                conn.Open();
                try
                {
                    string id = conn.QueryFirst<string>("SELECT ID FROM [USER] WHERE USERNAME = @USERNAME", new { username = cart.id_user });

                    string sqlquery = "select * FROM ITEMCART WHERE ID_USER = @USERNAME AND ID_BOOK = @BOOK";
                    CartItem cartD = conn.QueryFirstOrDefault<CartItem>(sqlquery, new { username = id, book = cart.id_book });
                    //return OK(usr[0].dat);
                    if (cartD != null)
                    {
                        if(cart.quantity > 0)
                        {
                            conn.Execute("UPDATE ITEMCART SET QUANTITY=@quantity WHERE ID_USER = @USERNAME AND ID_BOOK = @BOOK", new { username = id, book = cart.id_book, quantity = cart.quantity });
                        }
                        else
                        {
                            if (cart.quantity == 0)
                            {
                                conn.Execute("DELETE FROM ITEMCART WHERE ID_USER = @USERNAME AND ID_BOOK = @BOOK", new { username = id, book = cart.id_book });
                            }
                            else
                            {
                                return "Fail";
                            }
                        }
                    }
                    else
                    {
                        conn.Execute("INSERT INTO ITEMCART VALUES(@username,@id_book,@quantity)", new { username = id, id_book = cart.id_book, quantity = cart.quantity });
                    }
                    return "Success";
                }
                catch
                {
                    return "Fail";
                }
            }
        }
    }
}
