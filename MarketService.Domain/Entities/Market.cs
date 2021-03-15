using System;
using System.Collections.Generic;
using System.Text;

namespace MarketService.Domain.Entities
{
    public class Market
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public List<Company> Companies { get; set; }
    }
}
