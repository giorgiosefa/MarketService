using System;
using System.Collections.Generic;
using System.Text;

namespace MarketService.Domain.Model
{
    public class MarketBaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? ChangeDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
