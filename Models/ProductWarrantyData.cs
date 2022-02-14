using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WarrantyRegistrationApp.Models
{
    public class ProductWarrantyData
    {
        [Key]
        public int ProdWarrantyId { get; set; }
        public int CustomerId { get; set; }
        public string userId { get; set; }
        public int ProductId { get; set; }
        public string ProductSerialNumber { get; set; }
        public DateTime WarrantyDate {get;set;}
    }
}
