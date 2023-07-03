namespace BookStoreApi.Model
{
    public class Book
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public string author { get; set; }
        public string img { get; set; }
        public int stock { get; set; }

    }
}
