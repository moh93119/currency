using Currency.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Currency.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options)
    {
    }

    public DbSet<ConversionRate> ConversionRates { get; set; }
}