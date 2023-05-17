namespace BookStoreApi.Model
{
    public class User
    {
        public string? username { get; set; } = null;
        public string? password { get; set; } = null;
        public string? fullname { get; set; } = null;
        public string? token { get; set; } = null;

        public string? id { get; set; } = null;
        public string? salt { get; set; } = null;
    }

    public class InfoLogin
    {
        public string? username { get; set; } = null;
        public string? password { get; set; } = null;
    }
    public class InfoRegister
    {
        public string? username { get; set; } = null;
        public string? password { get; set; } = null;
        public string? fullname { get; set;} = null;
    }
}
