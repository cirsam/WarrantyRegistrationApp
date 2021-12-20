using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WarrantyRegistrationApp.Models
{
    public class Product
    {
        [Key]
        public int ProductId  {get;set;}
        public string ProductName { get; set; }
        public string Manufacturer { get; set; }
        public string ProductSerialNumber { get; set; }
    }
}
