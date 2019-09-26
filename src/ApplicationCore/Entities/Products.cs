using Ardalis.GuardClauses;
using System.Collections.Generic;

namespace ApplicationCore.Models
{
    public class Products
    {
        public Products(IEnumerable<Product> products)
        {
            Guard.Against.Null(products, nameof(products));
            Items = products;
        }
        public IEnumerable<Product> Items { get; private set; }
    }
}