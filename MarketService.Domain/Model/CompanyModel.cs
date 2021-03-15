using MarketService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketService.Domain.Model
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public int MarketId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public DateTime? ChangeDate { get; set; }
        public DateTime CreateDate { get; set; }

        public static explicit operator CompanyModel(Company company)
        {
            return new CompanyModel
            {
                Id = company.Id,
                MarketId = company.MarketId,
                Price = company.Price,
                Name = company.Name,
                ChangeDate = company.ChangeDate,
                CreateDate = company.CreateDate
            };
        }
    }
}
