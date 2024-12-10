using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ONIK_BANK.Models;

namespace ONIK_BANK.Data
{
    public class CustomerDbContext : IdentityDbContext<AppUser>
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
