using System;
using System.Collections.Generic;
using System.Text;

namespace MarketService.Domain.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public int MarketId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ChangeDate { get; set; }
        public Market Markets { get; set; }
    }
}
