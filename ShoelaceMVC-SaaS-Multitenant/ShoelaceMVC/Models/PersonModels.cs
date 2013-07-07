using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoelaceMVC.Models
{
    public class Person
    {
        public int Id { get; set; }
        public Account Account { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}