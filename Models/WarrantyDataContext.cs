using Microsoft.EntityFrameworkCore;

namespace WarrantyRegistrationApp.Models
{
    public class WarrantyDataContext:DbContext
    {
        public WarrantyDataContext(DbContextOptions<WarrantyDataContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductWarrantyData> ProductWarrantyDatas { get; set; }
    }
}
