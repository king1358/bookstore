using BookStoreApi.Interface;
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
        private readonly ICart _cartRepository;
        private readonly IUser _userRepository;
        public CartController(ICart cartRepository, IUser userRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartItem(int id, string token)
        {
            try
            {
                if (id == -1) return StatusCode(406, new { message = "Please login" });
                bool verify = await _userRepository.Verify(id,token);
                if (verify == false) return StatusCode(406, new { message = "You don't have premission to do this" });
                var id_cart = await _cartRepository.GetIdCart(id);
                if (id_cart != 0)
                {
                    List<CartItems> cartItems = await _cartRepository.GetAll(id_cart);
                    double totalCart = await _cartRepository.GetTotal(id_cart);
                    int amountItem = await _cartRepository.GetAmount(id_cart);
                    return Ok(new { cart = cartItems, total = totalCart, amount = amountItem });
                }
                object temp = null;
                return Ok(new { cart = temp });

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
                if (item.id_user == -1) return StatusCode(406, new { message = "Please login" });
                bool verify = await _userRepository.Verify(item.id_user,item.token);
                if (verify == false) return StatusCode(406, new { message = "You don't have premission to do this" });
                var id_cart = await _cartRepository.GetIdCart(item.id_user);
                if (id_cart == 0)
                {
                    
                    bool res = await _cartRepository.CreateCartForUser(item.id_user, item.id_user);
                    if (res == true)
                    {
                        bool res2 = await _cartRepository.InsertItem2Cart(item.id_user, item.id_book, item.amount, item.total);
                        if (res2 == true)
                        {

                            bool res3 = await _cartRepository.UpdateAmountAndTotalCart(item.id_user, item.total, item.amount);
                            if (res3 == true) return Ok("Sucess");
                            else return BadRequest("Error while add to cart");
                        }
                        else return BadRequest("Error while add to cart");
                    }
                    else return BadRequest("Error while add to cart");
                }
                else
                {
                    bool res = await _cartRepository.InsertItem2Cart(id_cart, item.id_book, item.amount, item.total);
                    if (res == true)
                    {
                        bool res2 = await _cartRepository.UpdateAmountAndTotalCart(id_cart, item.total, item.amount);
                        if (res2 == true) return Ok("Sucess");
                        else return BadRequest("Error while add to cart");
                    }
                    else return BadRequest("Error while add to cart");
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
                if (item.id_cart == -1) return StatusCode(406, new { message = "Please login" });
                bool verify = await _userRepository.Verify(item.id_user,item.token);
                if (verify == false) return StatusCode(406, new { message = "You don't have premission to do this" });
                double oldTotal = await _cartRepository.GetTotal(item.id_cart, item.id_book);
                int oldAmount = await _cartRepository.GetAmount(item.id_cart, item.id_book);
                bool res1 = await _cartRepository.UpdateItemCart(item.id_cart,item.id_book,item.amount, item.total);
                if (res1 == true)
                {
                    bool res2 = await _cartRepository.UpdateAmountAndTotalCart(item.id_cart, item.total - oldTotal, item.amount - oldAmount);
                //await _service.EditData("UPDATE CART SET TOTAL = TOTAL + @TOTAL - @OLDTOTAL WHERE ID = @id", new { total = item.total, oldtotal = oldTotal, id = item.id_c });
                    if (res2 == true)
                    {
                        double total = await _cartRepository.GetTotal(item.id_cart);
                        int amount = await _cartRepository.GetAmount(item.id_cart);
                        return Ok(new { result = "Sucess", total = total, amount = amount });
                    }
                    else return BadRequest("Error while update cart");
                }
                else return BadRequest("Error while update cart");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("removeItemCart")]
        public async Task<IActionResult> removeItemCart(int id_user,int id_cart, int id_book, string token)
        {
            try
            {
                if (id_cart == -1) return StatusCode(406, new { message = "Please login" });
                bool verify = await _userRepository.Verify(id_user,token);
                if (verify == false) return StatusCode(406, new { message = "You don't have premission to do this" });
                
                double total = await _cartRepository.GetTotal(id_cart,id_book);
                int amount = await _cartRepository.GetAmount(id_cart, id_book);
                bool res = await _cartRepository.DeleteItemCart(id_cart, id_book, total, amount);
                if (res == true)
                {
                    double totalCart = await _cartRepository.GetTotal(id_cart);
                    int amoutCart = await _cartRepository.GetAmount(id_cart);
                    return Ok(new { result = "Sucess", total = totalCart, amount= amoutCart });
                }
                else return BadRequest("Error while remove item cart");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
