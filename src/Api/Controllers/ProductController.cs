using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("products")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductV1Controller : ControllerBase
    {
         private readonly IProductRepository _productRepository;

        public ProductV1Controller(IProductRepository productRepository)
        {
            Guard.Against.Null(productRepository, nameof(productRepository));
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allProducts = await _productRepository.ListAllAsync();

        }
    }
}