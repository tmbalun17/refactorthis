using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Models
{
    public class ProductOptions
    {
        public ProductOptions(IEnumerable<ProductOption> productOptions)
        {
            Guard.Against.Null(productOptions, nameof(productOptions));
            Items = productOptions;
        }
        public IEnumerable<ProductOption> Items { get; private set; }
    }
}