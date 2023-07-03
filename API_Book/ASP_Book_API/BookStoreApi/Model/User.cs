namespace BookStoreApi.Model
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public string fullname { get; set; }

    }

    public class InfoLogin
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class InfoRegister
    {
        public string username { get; set; }
        public string password { get; set; }
        public string fullname { get; set;}
        public string email { get; set; }
        public string phone { get; set; }
        public DateTime birthDate { get; set; }

        public string housenumber { get; set; }
        public int id_province { get; set; }
        public int id_district { get; set; }
        public string id_ward { get; set; }
    }

    public class Address
    {
        public int id_user { get; set; }
        public int serial { get; set; }
        public int id_province { get; set; }
        public int id_district { get; set; }
        public string id_ward { get; set; }
        public string house_number { get; set; }
        public int is_default { get; set; }
        public string fullInfo { get; set; }
    }

    public class Province
    {
        public int id_province { get; set;}
        public string name { get; set; }
    }

    public class District
    {
        public int id_district { get; set; }
        public int id_province { get; set; }
        public string name { get; set; }
    }
    public class Ward
    {
        public string id_ward { get; set; }
        public int id_district { get; set; }
        public string name { get; set; }
    }
}
