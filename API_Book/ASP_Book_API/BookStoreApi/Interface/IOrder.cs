using BookStoreApi.Model;
using Newtonsoft.Json.Linq;

namespace BookStoreApi.Interface
{
    public interface IOrder
    {
        Task<List<PaymentMethod>> GetPaymentMethods();
        Task<string> InsertDataPaymentPayPal(CreateOrderReturnPayPal data, DateTime createTime);
        Task<int> InsertOrder(int id_user, int serial, double total, double feeship, string typeship, string shipinfo, string note, string id_payment);
        Task<bool> InsertOrderItem(List<DataOrder> listBook, int id_order, int id_cart, string id_event = null);
        Task<int> UpdatePayment(string token, string PayerID);
        Task<bool> UpdatePayment(int id_order);

        Task<bool> Verify(int id_order, string token,int id_user);
        Task<bool> VerifyUser(string token, int id_user);

        //Task<bool> Verify(int id_user, string token);

        Task<InfoOrderReturn> GetOrderInfo(int id_order);

        Task<int> GetIdOrder(string id_payment);

        Task<bool> checkExpires(int id_order);
        Task<List<OrderInfoSimple>> GetListOrders(int id_user);
    }

}
