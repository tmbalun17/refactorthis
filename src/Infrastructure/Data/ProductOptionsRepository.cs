using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class ProductOptionRepository : EfRepository<ProductOption>, IProductOptionRepository
    {
        public ProductOptionRepository(ProductContext dbContext) : base(dbContext)
        {
        }
    }
}
