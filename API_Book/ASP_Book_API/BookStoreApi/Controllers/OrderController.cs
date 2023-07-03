using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Interface;
using BookStoreApi.Model;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.VisualBasic;

namespace BookStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _orderRepository;
        private readonly IUser _userRepository;
        private readonly ICart _cartRepository;
        private string _tokenPaypal;
        private DateTime _timeRefesh;
        private string linkGHN;
        private string tokenGHN;
        private int idGHN;
        private string linkPayPal;
        private string userNamePayPal;
        private string passwordPayPal;
        private string returnPath;

        public OrderController(IOrder orderRepository, IUser userRepository, ICart cartRepository,IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            linkGHN = configuration["GHN:link"];
            tokenGHN = configuration["GHN:token"];
            idGHN = Int32.Parse(configuration["GHN:shopId"]);
            linkPayPal = configuration["PayPal:link"];
            userNamePayPal = configuration["PayPal:username"];
            passwordPayPal = configuration["PayPal:password"];
            returnPath = configuration["PayPal:returnPath"];
        }

        [HttpPost("Order")]
        public async Task<IActionResult> OrderBook(OrderData data)
        {
            try
            {
                if (data.payment == 1)
                {
                    _tokenPaypal = await GetToken();
                    CreateOrderReturnPayPal data2 = await CreateOrder(data);
                    data.item.data.RemoveAt(data.item.data.Count - 1);
                    string res1 = await _orderRepository.InsertDataPaymentPayPal(data2, DateTime.Now);
                    int serial = 1;
                    if (data.ship.address.is_new == true)
                        serial = await _userRepository.InsertAddressUser(data.id_user, data.ship.address.id_province,
                           data.ship.address.id_district, data.ship.address.id_ward, data.ship.address.house_number);
                    else serial = data.ship.address.serial;
                    int id_order = await _orderRepository.InsertOrder(data.id_user, serial, Convert.ToDouble(data.item.total), Convert.ToDouble(data.ship.infoShip.feeShip),
                        data.ship.infoShip.typeShip, data.ship.infoShip.shipInfo, data.ship.infoShip.note, res1);
                    int id_cart = await _cartRepository.GetIdCart(data.id_user);
                    bool res = await _orderRepository.InsertOrderItem(data.item.data, id_order, id_cart);

                    return Ok(new
                    {
                        result = "success",
                        link = data2.links[1].href,
                        id_order = id_order,
                    });
                }
                else
                {
                    return Ok(new { result = "ERROR" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ConfirmPayment")]
        public async Task<IActionResult> ConfirmPayment(string token, string PayerID)
        {
            try
            {
                _tokenPaypal = await GetToken();
                bool confirm = await ConfirmPaymentPalpal(token);
                if (confirm == false) return Ok(new { result = "fail", id_order = -1 });
                int id_order = await _orderRepository.UpdatePayment(token, PayerID);
                if (id_order == 0) return Ok(new { result = "fail", id_order = -1 });
                return Ok(new { result = "success", id_order = id_order });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("InfoOrder")]
        public async Task<IActionResult> GetInfoOrder(int id_order, string token,int id_user)
        {
            try
            {
                bool verify = await _orderRepository.Verify(id_order, token,id_user);
                if (verify == false) return StatusCode(406, new { result= "fail", message = "You don't have premission to do this" });
                bool checkExpires = await _orderRepository.checkExpires(id_order);
                //return : Info: total, fee,shipinfo,status,linkcheckout; OrderItem: ...
                if (checkExpires == false) await _orderRepository.UpdatePayment(id_order);
                InfoOrderReturn infoOrderReturn = await _orderRepository.GetOrderInfo(id_order);
                return Ok(new { result = "success", data = infoOrderReturn });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ListOrder")]
        public async Task<IActionResult> GetListOrder(int id_user, string token)
        {
            try
            {
                bool verify = await _orderRepository.VerifyUser(token, id_user);
                if (verify == false) return StatusCode(406, new { message = "You don't have premission to do this" });
                List<OrderInfoSimple> listOrder = await _orderRepository.GetListOrders(id_user);
                return Ok(new {message = "success", data =  listOrder});
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetShipInfo")]
        public async Task<IActionResult> GetShipInfo(int id_province, int id_district, string id_ward)
        {

            try
            {

                int id_service = await GetServiceShip(1452, id_district);
                BodyFee jsonBodyFee = new BodyFee(1452, id_service, null, id_district, id_ward, 1, 1, 1, 1, 0, 0, null);
                double fee = Convert.ToDouble(await GetFee(jsonBodyFee));
                double feeUSD = (double)(fee / 23525 * 1.0);
                feeUSD = Math.Round(feeUSD, 2);
                double feeUSDExpress = Math.Round(feeUSD * 2, 2);
                BodyDateShip jsonBodyDateShip = new BodyDateShip(1452, "21014", id_district, id_ward, id_service);
                long sec = await GetDayShip(jsonBodyDateShip);
                DateTime dateShip = new DateTime(1970, 1, 1).AddSeconds(sec);
                double timeCeil = Math.Ceiling((dateShip - DateTime.Now).TotalDays);
                double timeFloor = Math.Floor((dateShip - DateTime.Now).TotalDays);

                var Info = new List<ShipInfo>()
                {
                    new ShipInfo(){desc =  $"Standard shipping: {timeCeil} - {timeCeil+1} days with {feeUSD} USD" , fee = feeUSD, type = "Standard" },
                    new ShipInfo(){desc = $"Express shipping: {timeFloor} - {timeFloor+1} days with {feeUSDExpress} USD", fee = feeUSDExpress, type = "Express" }

                };
                return Ok(Info);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PaymentMethod")]
        public async Task<IActionResult> GetPaymentMethod()
        {
            try
            {
                List<PaymentMethod> list = await _orderRepository.GetPaymentMethods();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetId")]
        public async Task<IActionResult> GetId(string id_payment)
        {
            try
            {
                return Ok(await _orderRepository.GetIdOrder(id_payment));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<string> GetToken()
        {
            string baseUrl = $"{linkPayPal}/v1/oauth2/token";
            string userName = userNamePayPal;
            string password = passwordPayPal;
            using (HttpClient client = new HttpClient())
            {
                var byteArray = new UTF8Encoding().GetBytes($"{userName}:{password}");
                //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                var body = new[]
                {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                };
                using (HttpResponseMessage res = await client.PostAsync(baseUrl, new FormUrlEncodedContent(body)))
                {
                    using (HttpContent content = res.Content)
                    {

                        var data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            var obj = JsonSerializer.Deserialize<TokenReturn>(data);
                            _timeRefesh = DateTime.Now.AddSeconds(obj.expires_in);
                            return obj.access_token;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        private async Task<bool> ConfirmPaymentPalpal(string token)
        {
            string baseUrl = $"{linkPayPal}/v2/checkout/orders/{token}/capture";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_tokenPaypal}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var body = JsonContent.Create(new { a = 1});
                using (HttpResponseMessage res = await client.PostAsync(baseUrl, body))
                {
                    if (res.StatusCode == HttpStatusCode.Created)
                    {
                        return true;
                    }
                    else return false;
                }
            }
        }

        private DataCreatePayPal CreateDataOrderPayPal(OrderData data)
        {
            var item = new DataOrder()
            {
                name = "SHIP FEE",
                quantity = "1",
                unit_amount = new UnitAmount()
                {
                    currency_code = "USD",
                    value = data.ship.infoShip.feeShip
                }
            };
            data.item.data.Add(item);
            var paypalData = new DataCreatePayPal()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>()
                    {
                        new PurchaseUnit()
                        {
                            items = new List<DataOrder>(data.item.data),

                            amount = new AmountPayPal()
                            {
                                currency_code = "USD",
                                value = data.item.total,
                                breakdown = new BreakdownPayPal()
                                {
                                    item_total = new ItemTotalPayPal()
                                    {
                                        currency_code = "USD",
                                        value = data.item.total,
                                    }
                                }
                            },

                            shipping = new ShippingPayPal()
                            {
                                type = "SHIPPING",
                                name = new NamePayPal()
                                {
                                    full_name = data.ship.fullname
                                },
                                address = new AddressPayPal()
                                {
                                    address_line_1 = data.ship.address.address_line,
                                    admin_area_2 = "Việt Nam",
                                    country_code = "VN"
                                }
                            }
                        }
                    },
                application_context = new ApplicationContext()
                {
                    cancel_url = returnPath,
                    return_url = $"{returnPath}/completePayment"
                }
            };
            return paypalData;
        }



        private async Task<CreateOrderReturnPayPal> CreateOrder(OrderData dataOrder)
        {
            string baseUrl = $"{linkPayPal}/v2/checkout/orders/";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_tokenPaypal}");
                DataCreatePayPal dataCreate = CreateDataOrderPayPal(dataOrder);
                var body = JsonContent.Create(dataCreate);
                using (HttpResponseMessage res = await client.PostAsync(baseUrl, body))
                {
                    using (HttpContent content = res.Content)
                    {

                        var data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            var obj = JsonSerializer.Deserialize<CreateOrderReturnPayPal>(data);
                            return obj;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        private async Task<long> GetDayShip(BodyDateShip info)
        {
            
            string baseUrl = $"{linkGHN}/leadtime";
            string token = tokenGHN;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("token", token);
                var body = JsonContent.Create(info);
                using (HttpResponseMessage res = await client.PostAsync(baseUrl, body))
                {
                    using (HttpContent content = res.Content)
                    {

                        var data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            var obj = JsonSerializer.Deserialize<Ship>(data);
                            return obj.data.leadtime;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
        }
        private async Task<int> GetServiceShip(int from_district, int to_district)
        {
            int shop_id = idGHN;
            string baseUrl = $"{linkGHN}/available-services" +
                $"?shop_id={shop_id}&from_district={from_district}&to_district={to_district}";
            string token = tokenGHN;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("token", token);

                using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                {
                    using (HttpContent content = res.Content)
                    {

                        var data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            var obj = JsonSerializer.Deserialize<TypeReturn>(data);
                            return obj.data[0].service_id;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }
        }
        private async Task<int> GetFee(BodyFee info)
        {
            string shop_id = $"{idGHN}";
            string baseUrl = $"{linkGHN}/fee";
            string token = tokenGHN;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("token", token);
                client.DefaultRequestHeaders.Add("ShopId", shop_id);
                var body = JsonContent.Create(info);
                using (HttpResponseMessage res = await client.PostAsync(baseUrl, body))
                {
                    using (HttpContent content = res.Content)
                    {

                        var data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            var obj = JsonSerializer.Deserialize<FeeReturn>(data);
                            return obj.data.total;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }

        }
    }
}
