using ApplicationCore.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models
{
    public class ProductOption: BaseEntity
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public Guid ProductId { get; set; }

        [Required(AllowEmptyStrings=false, ErrorMessage = "Name is required.")]
        [StringLength(100)]
        [MinLength(1)]
        public string Name { get; set; }

        [StringLength(500)]
        [MinLength(1)]
        public string Description { get; set; }

    }
}