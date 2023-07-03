using BookStoreApi.Model;

namespace BookStoreApi.Interface
{
    public interface ICart
    {
        Task<List<CartItems>> GetAll(int id_user);
        Task<double> GetTotal(int id_cart);
        Task<double> GetTotal(int id_cart, int id_book);
        Task<int> GetAmount(int id_cart);
        Task<int> GetAmount(int id_cart, int id_book);

        Task<int> GetIdCart(int id_user);

        Task<int> GetCountCart(int id_user);
        Task<bool> CreateCartForUser(int id,int id_user);
        Task<bool> InsertItem2Cart(int id_cart, int id_book, int amount, double total);
        Task<bool> UpdateItemCart(int id_cart,int id_book,int amount,double total);
        Task<bool> UpdateAmountAndTotalCart(int id, double total, int amount);
        Task<bool> DeleteItemCart(int id_cart, int id_book, double totalDelete, int amountDelete);
    }
}
