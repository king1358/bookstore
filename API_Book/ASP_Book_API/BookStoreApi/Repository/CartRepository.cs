using BookStoreApi.Interface;
using BookStoreApi.Model;
using BookStoreApi.Service;
namespace BookStoreApi.Repository
{
    public class CartRepository : ICart
    {
        private readonly IService _service;
        public CartRepository(IService service)
        {
            _service = service;
        }
        
        public async Task<List<CartItems>> GetAll(int id_cart)
        {
            List<CartItems> items = await _service.GetAll<CartItems>("SELECT * FROM CART_ITEM WHERE ID_CART = @ID_CART",
                new { id_cart = id_cart });
            return items;
        }
        public async Task<double> GetTotal(int id_cart)
        {
            return await _service.GetAsync<double>("SELECT TOTAL FROM CART WHERE ID = @ID",
                new { id = id_cart });
        }

        public async Task<double> GetTotal(int id_cart,int id_book)
        {
            return await _service.GetAsync<double>("SELECT TOTAL FROM CART_ITEM WHERE ID_CART = @ID_CART AND ID_BOOK = @ID_BOOK", 
                new { id_cart = id_cart, id_book = id_book });
        }

        public async Task<int> GetAmount(int id_cart)
        {
            return await _service.GetAsync<int>("SELECT AMOUNT FROM CART WHERE ID = @ID", new { id = id_cart });
        }
        public async Task<int> GetAmount(int id_cart, int id_book)
        {
            return await _service.GetAsync<int>("SELECT AMOUNT FROM CART_ITEM WHERE ID_CART = @ID_CART AND ID_BOOK = @ID_BOOK",
                new { id_cart = id_cart, id_book = id_book });
        }

        public async Task<int> GetIdCart(int id_user)
        {
            return await _service.GetAsync<int>("SELECT ID FROM CART WHERE ID_USER = @id", new { id = id_user });
            
        }

        public async Task<int> GetCountCart(int id_user)
        {
            return await _service.GetAsync<int>("SELECT COUNT(*) FROM CART WHERE ID_USER = @id", new { id = id_user });
        }
        public async Task<bool> CreateCartForUser(int id, int id_user)
        {
            int n = await _service.EditData("INSERT INTO CART VALUES(@ID,0,0,@ID_USER)", new { id = id, id_user = id_user });
            return (n == 1);           
        }

        public async Task<bool> InsertItem2Cart(int id_cart, int id_book, int amount, double total)
        {
            int n = await _service.EditData("INSERT INTO CART_ITEM VALUES(@ID_CART,@ID_BOOK,@AMOUNT,@TOTAL)",
                        new { id_cart = id_cart, id_book = id_book, amount = amount, total = total });
            return (n == 1);
        }

        public async Task<bool> UpdateItemCart(int id_cart,int id_book,int amount, double total)
        {
           int n = await _service.EditData("UPDATE CART_ITEM SET AMOUNT = @AMOUNT, TOTAL = @TOTAL WHERE ID_CART = @ID_CART AND ID_BOOK = @ID_BOOK", new { amount = amount, total = total, id_cart = id_cart, id_book = id_book });
            return (n == 1);
        }

        public async Task<bool> UpdateAmountAndTotalCart(int id, double total, int amount)
        {
            int n = await _service.EditData("UPDATE CART SET TOTAL = TOTAL + @TOTAL, AMOUNT = AMOUNT + @AMOUNT WHERE ID = @id", 
                new { total = total, amount = amount, id = id });
            return (n == 1);
        }
        public async Task<bool> DeleteItemCart(int id_cart,int id_book,double totalDelete, int amountDelete)
        {
            int n = await _service.EditData("DELETE CART_ITEM WHERE ID_CART = @ID_CART AND ID_BOOK = @ID_BOOK", new { id_cart = id_cart, id_book = id_book });
            if (n == 1)
            {
                int m = await _service.EditData("UPDATE CART SET TOTAL = TOTAL  - @TOTALDELETE, AMOUNT = AMOUNT - @AMOUNTDELETE WHERE ID = @id", new { totalDelete = totalDelete, amountDelete = amountDelete, id = id_cart });
                return (m == 1);
            }
            else return false;
        }
    }
}
