using ApplicationCore.Entities;
using System;

namespace ApplicationCore.Models
{
    public class ProductOption: BaseEntity
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}