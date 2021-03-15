using System;
using System.Collections.Generic;
using System.Text;

namespace MarketService.Persistence.Database
{
    public class DbInitializer
    {
        public static void Initialize(MarketDbContext dbcontext)
        {
            dbcontext.Database.EnsureCreated();
        }
    }
}
