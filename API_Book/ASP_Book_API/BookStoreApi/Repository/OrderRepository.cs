using BookStoreApi.Interface;
using BookStoreApi.Model;
using BookStoreApi.Service;

namespace BookStoreApi.Repository
{
    public class OrderRepository : IOrder
    {
        private readonly IService _service;
        private readonly ICart _cartRepository;
        private readonly IUser _userRepository;
        public OrderRepository(IService service, ICart cartRepository, IUser userRepository)
        {
            _service = service;
            _cartRepository = cartRepository;
            _userRepository = userRepository;
        }
        public async Task<List<PaymentMethod>> GetPaymentMethods()
        {
            return await _service.GetAll<PaymentMethod>("SELECT * FROM PAYMENT_METHOD", new { });
        }
        public async Task<string> InsertDataPaymentPayPal(CreateOrderReturnPayPal data,DateTime createTime)
        {
            int n = await _service.EditData("INSERT INTO PAYMENT_PAYPAL VALUES(@ID_PAYMENT,'Pending',@CREATETIME,@EXPIRES_IN," +
                "@LINKCHECKOUT,@LINKCHECK,NULL)",
                new
                {
                    id_payment = data.id,
                    createtime = createTime,
                    expires_in = 3600 * 6,
                    linkcheckout = data.links[1].href,
                    linkcheck = data.links[0].href
                });
            return data.id;
        }
        public async Task<int> InsertOrder(int id_user,int serial,double total, double feeship, string typeship,string shipinfo, string note,string id_payment)
        {
            int id = await _service.GetAsync<int>("SELECT COUNT(*) FROM [ORDER]", new { });
            id += 1;
            int n = await _service.EditData("INSERT INTO [ORDER] VALUES(@ID,@ID_USER,@SERIAL,@TOTAL,@FEESHIP,@TYPESHIP,@SHIPINFO,@NOTE," +
                "'Waiting payment',NULL,@ID_PAYMENT,'Paypal')", new
                {
                    id = id,
                    id_user = id_user,
                    serial = serial,
                    total = total,
                    feeship = feeship,
                    typeship = typeship,
                    shipinfo = shipinfo,
                    note = note,
                    id_payment = id_payment
                });
            return id;
            
        }
        public async Task<bool> InsertOrderItem(List<DataOrder> listBook,int id_order,int id_cart,string id_event = null)
        {
            foreach(DataOrder book in listBook)
            {
                
                int n = await _service.EditData("INSERT INTO ORDER_ITEM VALUES(@ID_ORDER,@ID_BOOK,@AMOUNT,@TOTAL,@ID_EVENT,NULL)",
                    new
                    {
                        id_order = id_order,
                        id_book = book.id_book,
                        amount = book.quantity,
                        total = book.unit_amount.value,
                        id_event = id_event
                    });
                if (n == 1)
                {
                    int m = await _service.EditData("DELETE CART_ITEM WHERE ID_CART = @ID_CART AND ID_BOOK = @ID_BOOK ", new
                    {
                        id_cart = id_cart,
                        id_book = book.id_book
                    });
                    if (m == 1) await _cartRepository.UpdateAmountAndTotalCart(id_cart, (Convert.ToDouble(book.unit_amount.value) * Convert.ToDouble(book.quantity)) * -1.0, Convert.ToInt16(book.quantity) * -1);
                }
            }
            return true;
        }
        public async Task<int> UpdatePayment(string token, string PayerID)
        {
            int id_order = await _service.GetAsync<int>("SELECT ID_ORDER FROM [ORDER] WHERE ID_PAYMENT = @TOKEN AND STATUS = 'Waiting payment'",
                new { token = token });
            int n = await _service.EditData("UPDATE [ORDER] SET STATUS = 'shipping' WHERE ID_PAYMENT = @TOKEN", new { token = token });
            int m = await _service.EditData("UPDATE PAYMENT_PAYPAL SET ID_PAYER = @PAYERID,STATUS='Paid' WHERE ID_PAYMENT = @TOKEN", 
                new { token = token , payerID = PayerID });
            return id_order;
        }

        public async Task<bool> UpdatePayment(int id_order)
        {
            string id_payment = await _service.GetAsync<string>("SELECT ID_PAYMENT FROM [ORDER] WHERE ID_ORDER = @ID_ORDER",
                new { id_order = id_order });
            int n = await _service.EditData("UPDATE [ORDER] SET STATUS = 'Cancel', REASON = 'Not payment' WHERE ID_ORDER = @ID_ORDER",
                new { id_order = id_order });
            int m = await _service.EditData("UPDATE PAYMENT_PAYPAL SET STATUS = 'Cancel', LINKCHECKOUT = NULL, LINKCHECK = NULL WHERE ID_PAYMENT = @ID_PAYMENT", 
                new { id_payment  = id_payment });
            return (n == 1 && m == 1);
        }
        public async Task<bool> Verify(int id_order, string token, int id_user)
        {
            string username = await _service.GetAsync<string>("SELECT USERNAME FROM [USER] U JOIN [ORDER] O " +
                "ON U.ID = O.ID_USER AND U.TOKEN = @TOKEN AND O.ID_ORDER = @ID_ORDER AND U.ID = @ID_USER",
                new { token = token, id_order = id_order, id_user = id_user });
            if (username == null) return false;
            return true;
        }
        //public async Task<bool> Verify(int id_user, string token)
        //{
        //    string username = await _service.GetAsync<string>("SELECT USERNAME FROM [USER] WHERE ID = @ID_USER AND TOKEN = @TOKEN",
        //        new {id_user = id_user, token = token});
        //    if (username == null) return false;
        //    return true;
        //}

        public async Task<bool> checkExpires(int id_order) {
            var infoCheck1 = await _service.GetAsync<checkExpiresTemp1>("SELECT ID_PAYMENT, TYPE_PAYMENT FROM [ORDER] WHERE ID_ORDER = @ID_ORDER",
                new { id_order = id_order });
            string status = await _service.GetAsync<string>("SELECT STATUS FROM PAYMENT_PAYPAL WHERE ID_PAYMENT = @ID_PAYMENT",
                new { id_payment = infoCheck1.id_payment });
            if (status == "Paid") return true;
            if (status == "Cancel") return false;
            checkExpiresTemp2 infoCheck2 = new checkExpiresTemp2();
            if (infoCheck1.type_payment == "Paypal")
                infoCheck2 = await _service.GetAsync<checkExpiresTemp2>("SELECT CREATETIME,EXPIRES_IN FROM PAYMENT_PAYPAL WHERE ID_PAYMENT = @ID_PAYMENT",
                    new { id_payment = infoCheck1.id_payment });
            if (DateTime.Now > infoCheck2.createTime.AddSeconds(infoCheck2.expires_in)) return false;
            return true;
        }

        public async Task<InfoOrderReturn> GetOrderInfo(int id_order)
        {
            //return : Info: total, fee,shipinfo,status,linkcheckout, reason, payment type, address, fullname; OrderItem: ...
            Order infoReturn = await _service.GetAsync<Order>("SELECT * FROM [ORDER] WHERE ID_ORDER = @ID_ORDER",
                new { id_order = id_order });
            string fullname = await _service.GetAsync<string>("SELECT FULLNAME FROM [USER] WHERE ID = @ID_USER",
                new { id_user = infoReturn.id_user });
            Address address = await _service.GetAsync<Address>("SELECT * FROM ADDRESS_USER WHERE ID_USER = @ID_USER AND SERIAL = @SERIAL ",
                new { id_user = infoReturn.id_user, serial = infoReturn.serial });
            string addressFullInfo = await _userRepository.GetFullInfoAddress(address);
            List<ItemOrderReturn> listItem = await _service.GetAll<ItemOrderReturn>("SELECT ID_BOOK,AMOUNT,TOTAL FROM ORDER_ITEM WHERE ID_ORDER = @ID_ORDER",
                new { id_order = id_order });
            string linkCheckOut = "a";
            if (infoReturn.type_payment == "Paypal")
            {
                linkCheckOut = await _service.GetAsync<string>("SELECT LINKCHECKOUT FROM PAYMENT_PAYPAL WHERE ID_PAYMENT = @ID_PAYMENT",
                                new { id_payment = infoReturn.id_payment });
            }

            InfoOrderReturn infoOrderReturn = new InfoOrderReturn()
            {
                info = new InfoReturn()
                {
                    address = addressFullInfo,
                    fee = infoReturn.feeship,
                    shipInfo = infoReturn.shipInfo,
                    status = infoReturn.status,
                    linkCheckOut = linkCheckOut,
                    note = infoReturn.note,
                    reason = infoReturn.reason,
                    total = infoReturn.total,
                    typePayment = infoReturn.type_payment,
                    typeShip = infoReturn.typeship,
                    fullname = fullname
                },
                item = listItem
            };
            return infoOrderReturn;
        }
        public async Task<int> GetIdOrder(string id_payment)
        {
            return await _service.GetAsync<int>("SELECT ID_ORDER FROM [ORDER] WHERE ID_PAYMENT = @ID_PAYMENT",
                new {id_payment = id_payment});
        }

        public async Task<bool> VerifyUser(string token, int id_user)
        {
            string username = await _service.GetAsync<string>("SELECT USERNAME FROM [USER] WHERE ID = @ID_USER AND TOKEN = @TOKEN",
                new { id_user = id_user, token = token });
            if (username == null) return false;
            return true;
        }
        public async Task<List<OrderInfoSimple>> GetListOrders(int id_user)
        {
            List<OrderInfoSimple> listOrder = await _service.GetAll<OrderInfoSimple>("SELECT ID_ORDER,TOTAL,STATUS FROM [ORDER] WHERE ID_USER = @ID_USER",
                new { id_user = id_user, });
            return listOrder;
        }

    }
}
