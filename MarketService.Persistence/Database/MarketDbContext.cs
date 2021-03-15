using MarketService.Domain.Entities;
using MarketService.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketService.Persistence.Database
{
    public class MarketDbContext: DbContext
    {
        public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options)
        {

        }

        public DbSet<Market> Markets { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new MarketsConfiguration());
            builder.ApplyConfiguration(new CompaniesConfiguration());
        }
    }
}
