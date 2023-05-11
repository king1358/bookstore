using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class User
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public string birthdate    { get; set; }
        public string address { get; set; }
        public string token { get; set; }
        public string status { get; set; }
        public string role { get; set; }
        public string createdate { get; set; }

        public string salt { get; set; }
       
    }
}
