using System;
using System.Collections.Generic;
using System.Text;

namespace MarketService.Domain.Model
{
    public class CompanyRequestModel
    {
        public int MarketId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
    }
}
