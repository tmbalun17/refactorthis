using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model
{
    public class NewProduct
    {
        [Required(AllowEmptyStrings=false, ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(500, MinimumLength = 1)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Delivery Price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public decimal DeliveryPrice { get; set; }
    }
}
