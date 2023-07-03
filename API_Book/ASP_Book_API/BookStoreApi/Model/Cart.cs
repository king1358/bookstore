namespace BookStoreApi.Model
{
    public class Cart
    {
        public int id { get; set; }
        public double total { get; set; }
        public int id_user { get; set; }
        public int amount { get; set; }
    }

    public class CartItems
    {
        public int id_cart { get; set; }
        public int id_book { get; set; }
        public int amount { get; set; }
        public double total { get; set; }
    }

    public class InfoAdd
    {
        public int id_user { get; set; }
        public int id_book { get; set; }

        public int amount { get; set; }
        public double total { get; set; }

        public string token { get; set; }
    }

    public class InfoUpdate
    {
        public int id_user { get; set; }
        public int id_cart { get; set; }
        public int id_book { get; set; }

        public int amount { get; set; }
        public double total { get; set; }

        public string token { get; set; }
    }
    public class InfoCheckOut
    {
        public int id_user { get; set; }
        public double total { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string noted { get; set; }
        public DateTime timeOrder { get; set; }
        public double totalSale { get; set; }
        public string paymentMethod { get; set; }
        public string token { get; set; }
    }
    public class Item
    {
        public string name { get; set; }
        public string id_book { get; set; }
        public int amount { get; set; }
        public int stock { get; set; }
    }
}
