namespace BookStoreApi.Model
{
    public class Order
    {
        public int id_order { get; set; }
        public int id_user { get; set; }
        public int serial { get; set; }
        public double total { get; set; }
        public double feeship { get; set; }
        public string typeship { get; set; }
        public string shipInfo { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
        public string id_payment { get; set; }
        public string type_payment { get; set; }
        public string note { get; set; }


    }
    public class OrderData
    {
        public ItemOrder item { get; set; }
        public ShipOrder ship { get; set; }
        public int payment { get; set; }
        public int id_user { get; set; }
        public string token { get; set; }
    }

    public class ItemOrder
    {
        public List<DataOrder> data { get; set; }
        public string total { get; set; }
    }

    public class ShipOrder
    {
        public string fullname { get; set; }
        public AddressOrder address { get; set; }
        public InfoShipOrder infoShip { get; set; }
    }

    public class AddressOrder
    {
        public string address_line { get; set; }
        public int id_province { get; set; }
        public int id_district { get; set; }
        public string id_ward { get; set; }
        public string house_number { get; set; }
        public int serial { get; set; }
        public bool is_new { get; set; }
    }
    public class InfoShipOrder
    {
        public string feeShip { get; set; }
        public string typeShip { get; set; }
        public string shipInfo { get; set; }
        public string? note { get; set; }
    }

    public class DataOrder
    {
        public int id_book { get; set; }
        public string name { get; set; }
        public string quantity { get; set; }
        public UnitAmount unit_amount { get; set; }
        
    }

    public class UnitAmount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class ShipInfo { 
        public string desc { get; set; }
        public double fee { get; set; }
        public string type { get; set; }
    }

    public class PaymentMethod
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class AddressUserInfo
    {
        public int id_province { get; set; }
        public int id_district { get; set; }
        public string id_ward { get; set; }
    }
    public class Ship
    {
        public int code { get; set; }
        public string name { get; set; }
        public DataShip data { get; set; }

    }
    public class DataShip
    {
        public long leadtime { get; set; }
        public long order_date { get; set; }
    }

    public class BodyDateShip
    {
        public int from_district_id { get; set; }
        public string from_ward_code { get; set; }
        public int to_district_id { get; set; }
        public string to_ward_code { get; set; }
        public int service_id { get; set; }
        public BodyDateShip(int from_district_id, string from_ward_code, int to_district_id, string to_ward_code, int service_id)
        {
            this.from_district_id = from_district_id;
            this.from_ward_code = from_ward_code;
            this.to_district_id = to_district_id;
            this.to_ward_code = to_ward_code;
            this.service_id = service_id;
        }
    }

    public class TypeReturn
    {
        public int code { get; set; }
        public string code_message_value { get; set; }
        public List<DataType> data { get; set; }
    }

    public class DataType
    {
        public int service_id { get; set; }
        public string short_name { get; set; }
        public int service_type_id { get; set; }
        public string config_fee_id { get; set; }
        public string extra_cost_id { get; set; }
        public string standard_config_fee_id { get; set; }
        public string standard_extra_cost_id { get; set; }
    }

    public class BodyFee
    {
        public int from_district_id { get; set; }
        public int service_id { get; set; }
        public object service_type_id { get; set; }
        public int to_district_id { get; set; }
        public string to_ward_code { get; set; }
        public int height { get; set; }
        public int length { get; set; }
        public int weight { get; set; }
        public int width { get; set; }
        public int cod_failed_amount { get; set; }
        public int insurance_value { get; set; }
        public object coupon { get; set; }

        public BodyFee(int from_district_id, int service_id, object service_type_id, int to_district_id, string to_ward_code, int height, int length, int weight, int width, int cod_failed_amount, int insurance_value, object coupon)
        {
            this.from_district_id = from_district_id;
            this.service_id = service_id;
            this.service_type_id = service_type_id;
            this.to_district_id = to_district_id;
            this.to_ward_code = to_ward_code;
            this.height = height;
            this.length = length;
            this.weight = weight;
            this.width = width;
            this.cod_failed_amount = cod_failed_amount;
            this.insurance_value = insurance_value;
            this.coupon = coupon;
        }


    }
    public class DataFee
    {
        public int total { get; set; }
        public int service_fee { get; set; }
        public int insurance_fee { get; set; }
        public int pick_station_fee { get; set; }
        public int coupon_value { get; set; }
        public int r2s_fee { get; set; }
        public int return_again { get; set; }
        public int document_return { get; set; }
        public int double_check { get; set; }
        public int cod_fee { get; set; }
        public int pick_remote_areas_fee { get; set; }
        public int deliver_remote_areas_fee { get; set; }
        public int cod_failed_fee { get; set; }
    }

    public class FeeReturn
    {
        public int code { get; set; }
        public string message { get; set; }
        public DataFee data { get; set; }
    }
    public class TokenReturn
    {
            public string scope { get; set; }
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string app_id { get; set; }
            public int expires_in { get; set; }
            public string nonce { get; set; }
    }

    //return : Info: total, fee,shipinfo,status,linkcheckout; OrderItem: ...

    public class InfoOrderReturn
    {
        public InfoReturn info { get; set; }
        public List<ItemOrderReturn> item { get; set; }
    }

    public class InfoReturn
    {
        public double total { get; set; }
        public double fee { get; set; }
        public string shipInfo { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
        public string linkCheckOut { get; set; }
        public string typeShip { get; set; }
        public string note { get; set; }
        public string address { get; set; }
        public string typePayment { get; set; }
        public string fullname { get; set; }
    }

    

    public class ItemOrderReturn
    {
        public int id_book { get; set; }
        public int amount { get; set; }
        public double total { get; set; }
    }
    
    public class checkExpiresTemp1
    {
        public string id_payment { get; set; }
        public string type_payment { get; set; }
    }
    public class checkExpiresTemp2
    {
        public DateTime createTime { get; set; }
        public int expires_in { get; set; }
    }

    public class OrderInfoSimple
    {
        public int id_order { get; set; }
        public double total { get; set; }
        public string status { get; set; }
    }
}
