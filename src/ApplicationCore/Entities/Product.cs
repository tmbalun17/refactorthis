using ApplicationCore.Entities;
using System;

namespace ApplicationCore.Models
{
    public class Product: BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}