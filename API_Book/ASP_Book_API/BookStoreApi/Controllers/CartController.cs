using BookStoreApi.Model;
using BookStoreApi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IService _service;
        public CartController(IService dbService)
        {
            _service = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItem(string id)
        {
            try
            {
                if (id == "NoneLogin") return BadRequest("Please login");
                List<CartItems> cartItems = await _service.GetAll<CartItems>("SELECT CART_ITEM.* FROM CART_ITEM JOIN CART ON CART_ITEM.ID_C = CART.ID AND CART.ID_U = @ID WHERE CART.STATUS = 'wait'", new { id = id });
                int totalCart = await _service.GetAsync<int>("SELECT TOTAL FROM CART WHERE ID_U = @ID AND STATUS = 'wait'", new {id =  id});
                return Ok(new { cart = cartItems, total = totalCart });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("addCart")]
        public async Task<IActionResult> addCartItem([FromBody] InfoAdd item)
        {
            try
            {
                string username = await _service.GetAsync<string>("SELECT USERNAME FROM [USER] WHERE TOKEN = @TOKEN", new { token = item.token });
                if (username == null) return BadRequest("You don't have premission to do this");
                string id = await _service.GetAsync<string>("SELECT ID FROM CART WHERE ID_U = @id AND STATUS = 'wait'", new { id = item.id_u });
                if (id == null)
                {
                    int number = await _service.GetAsync<int>("SELECT COUNT(*) FROM CART WHERE ID_U = @id", new {id =  item.id_u});
                    number += 1;
                    string id_c = $"C_{item.id_u}_{number}";
                    int n = await _service.EditData("INSERT INTO CART VALUES(@ID,0,@ID_U,'wait',NULL)", new { id = id_c, id_u = item.id_u });
                    if (n == 1)
                    {
                        int m = await _service.EditData("INSERT INTO CART_ITEM VALUES(@ID_C,@ID_B,@AMOUNT,@TOTAL)",
                        new { id_c = id_c, id_b = item.id_b, amount = item.amount, total = item.total });
                        int l = await _service.EditData("UPDATE CART SET TOTAL = TOTAL + @TOTAL WHERE ID_U = @id", new { total = item.total, id = item.id_u });
                        if (m == 1) return Ok("Sucess");
                        else return Ok("Error while add to cart");
                    }
                    else return Ok("Error while add to cart");
                }
                else 
                {
                    int m =  await _service.EditData("INSERT INTO CART_ITEM VALUES(@ID_C,@ID_B,@AMOUNT,@TOTAL)",
                        new { id_c = id, id_b = item.id_b, amount = item.amount, total = item.total });
                    await _service.EditData("UPDATE CART SET TOTAL = TOTAL + @TOTAL WHERE ID_U = @id", new { total = item.total, id = item.id_u });
                    if (m == 1) return Ok("Sucess");
                    else return Ok("Error while add to cart");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updateCart")]
        public async Task<IActionResult> updateCart([FromBody] InfoUpdate item)
        {
            try
            {
                string username = await _service.GetAsync<string>("SELECT USERNAME FROM [USER] WHERE TOKEN = @TOKEN", new { token = item.token });
                if (username == null) return BadRequest("You don't have premission to do this");
                int oldTotal = await _service.GetAsync<int>("SELECT TOTAL FROM CART_ITEM WHERE ID_C = @ID_C AND ID_B = @ID_B", new { id_c = item.id_c, id_b = item.id_b });
                int n = await _service.EditData("UPDATE CART_ITEM SET AMOUNT = @AMOUNT, TOTAL = @TOTAL WHERE ID_C = @ID_C AND ID_B = @ID_B", new {amount = item.amount, total = item.total, id_c = item.id_c, id_b = item.id_b });
                await _service.EditData("UPDATE CART SET TOTAL = TOTAL + @TOTAL - @OLDTOTAL WHERE ID = @id", new { total = item.total,oldtotal = oldTotal, id = item.id_c });
                int totalCart = await _service.GetAsync<int>("SELECT TOTAL FROM CART WHERE ID = @ID", new { id = item.id_c });
                if (n == 1) return Ok(new { result = "Sucess", total = totalCart});
                else return Ok("Error while update cart");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("RemoveItemCart")]
        public async Task<IActionResult> removeItemCart(string id_c,string id_b,string token)
        {
            try
            {
                string username = await _service.GetAsync<string>("SELECT USERNAME FROM [USER] WHERE TOKEN = @TOKEN", new { token = token });
                if (username == null) return BadRequest("You don't have premission to do this");
                int totalItem = await _service.GetAsync<int>("SELECT TOTAL FROM CART_ITEM WHERE ID_C = @ID_C AND ID_B = @ID_B", new { id_c = id_c, id_b = id_b });
                int n = await _service.EditData("DELETE CART_ITEM WHERE ID_C = @ID_C AND ID_B = @ID_B", new { id_c = id_c, id_b = id_b });
                await _service.EditData("UPDATE CART SET TOTAL = TOTAL  - @TOTALITEMR WHERE ID = @id", new { totalitemr = totalItem , id = id_c });
                int totalCart = await _service.GetAsync<int>("SELECT TOTAL FROM CART WHERE ID = @ID AND STATUS = 'wait'", new { id = id_c });
                if (n == 1) return Ok(new { result = "Sucess", total = totalCart });
                else return Ok("Error while remove item cart");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Checkout")]
        public async Task<IActionResult> checkOut([FromBody] InfoCheckOut checkOutDetail)
        {
            try
            {
                string username = await _service.GetAsync<string>("SELECT USERNAME FROM [USER] WHERE TOKEN = @TOKEN", new { token = checkOutDetail.token });
                if (username == null) return BadRequest("You don't have premission to do this");
                string id_c = await _service.GetAsync<string>("SELECT ID FROM CART WHERE STATUS = 'wait' AND ID_U = @ID_U", new {id_u = checkOutDetail.id_u });
                DateTime now = DateTime.Now;
                int n = await _service.EditData("UPDATE CART SET STATUS = 'delivery',TIME_CHECKOUT = @NOW WHERE ID = @ID", new { now = now,id = id_c });
                if (n == 1) return Ok("Done");
                else return Ok("Error while checkout");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
