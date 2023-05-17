namespace BookStoreApi.Model
{
    public class Cart
    {
        public string? id { get; set; } = null;
        public double? total { get; set; } = null;
        public string? id_u { get; set; } = null;
        public string? status { get; set; } = null;
        public DateTime? timeCheckOut { get; set; } = null;
    }

    public class CartItems
    {
        public string? id_c { get; set; } = null;
        public string? id_b { get; set; } = null;
        public int? amount { get; set; } = null;
        public string? total { get; set; } = null;
    }

    public class InfoAdd
    {
        public string? id_u { get; set; } = null;
        public string? id_b { get; set; } = null;

        public int? amount { get; set; } = null;
        public double? total { get; set; } = null;

        public string? token { get; set; } = null;
    }

    public class InfoUpdate
    {
        public string? id_c { get; set; } = null;
        public string? id_b { get; set; } = null;

        public int? amount { get; set; } = null;
        public double? total { get; set; } = null;

        public string? token { get; set; } = null;
    }
    public class InfoCheckOut
    {
        public string? id_u { get; set; } = null;
        public string? token { get; set; } = null;
    }
}
