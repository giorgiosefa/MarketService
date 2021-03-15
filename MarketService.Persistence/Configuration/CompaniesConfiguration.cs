using MarketService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketService.Persistence.Configuration
{
    public class CompaniesConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.MarketId).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Name).HasColumnType("nvarchar(50)").IsRequired();            
            builder.Property(x => x.CreateDate).IsRequired().HasDefaultValueSql("getdate()");
            builder.Property(x => x.ChangeDate).IsRequired(false);
        }
    }
}
