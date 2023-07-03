namespace BookStoreApi.Model
{
    public class DataCreatePayPal
    {
        public string intent { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; }
        public ApplicationContext application_context { get; set; }
    }

    public class ApplicationContext
    {
        public string return_url { get; set; }
        public string cancel_url { get; set; }
    }
    public class PurchaseUnit
    {
        public List<DataOrder> items { get; set; }
        public AmountPayPal amount { get; set; }
        public ShippingPayPal shipping { get; set; }
    }

    public class AmountPayPal
    {
        public string currency_code { get; set; }
        public string value { get; set; }
        public BreakdownPayPal breakdown { get; set; }
    }
    public class BreakdownPayPal
    {
        public ItemTotalPayPal item_total { get; set; }
    }

    public class ItemTotalPayPal
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }
    public class ShippingPayPal
    {
        public string type { get; set; }
        public AddressPayPal address { get; set; }
        public NamePayPal name { get; set; }
    }

    public class NamePayPal
    {
        public string full_name { get; set; }
    }

    public class AddressPayPal
    {
        public string address_line_1 { get; set; }
        public string admin_area_2 { get; set; }
        public string country_code { get; set; }
    }

    public class CreateOrderReturnPayPal
    {
        public string id { get; set; }
        public string status { get; set; }
        public List<CreateOrderLinkReturn> links { get; set; }
    }
    public class CreateOrderLinkReturn
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }

}
