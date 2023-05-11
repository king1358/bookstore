namespace WebApplication1.Models
{

    public class CartItem
    {
        public string id_user { get; set; }
        public string id_book { get; set; }
        public int quantity { get; set; }
    }

    public class ResultCart
    {
        public string result { get; set; }
        public List<CartItem> items { get; set; } = new List<CartItem>();
    }
}
