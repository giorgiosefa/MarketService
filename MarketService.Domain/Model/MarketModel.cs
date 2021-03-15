using MarketService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketService.Domain.Model
{
    public class MarketModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? ChangeDate { get; set; }
        public DateTime CreateDate { get; set; }
        public List<CompanyModel> Company { get; set; }

        public static explicit operator MarketModel(Market market)
        {
            return new MarketModel
            {
                Id = market.Id,
                Name = market.Name,
                ChangeDate = market.ChangeDate,
                CreateDate = market.CreateDate,
                Company = market.Companies?.Select(x => (CompanyModel)x).ToList()
            };
        }
    }
}
