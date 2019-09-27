using Api.Model;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("products")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductV1Controller : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductOptionRepository _productOptionsRepository;

        public ProductV1Controller(IProductRepository productRepository, IProductOptionRepository productOptionsRepository)
        {
            Guard.Against.Null(productRepository, nameof(productRepository));
            Guard.Against.Null(productOptionsRepository, nameof(productOptionsRepository));
            _productRepository = productRepository;
            _productOptionsRepository = productOptionsRepository;

        }

        #region Product CRUD
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery]string name)
        {
            IEnumerable<Product> allProducts = String.IsNullOrEmpty(name) ?
                                await _productRepository.ListAllAsync()
                                : await _productRepository.ListAsync(p => p.Name == name);
            
            if(allProducts == null)
                return null;

            var products = new Products(allProducts);
            return Ok(products);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetProductAsync(Guid id)
        {
            Guard.Against.NullGuid(id, nameof(id));
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(NewProduct product)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
                return BadRequest(ModelState);
            }

            // Can use AutoMapper normally but ignoring for this
            var productModel = new ApplicationCore.Models.Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice
            };
            var savedProduct = await _productRepository.AddAsync(productModel);
            return CreatedAtAction(nameof(GetProductAsync), new { id = savedProduct.Id }, savedProduct);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(Product product)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
                return BadRequest(ModelState);
            }


            await _productRepository.UpdateAsync(product);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            //TODO: Can be done using cascade delete or this needs a transaction but for now will do
            await _productOptionsRepository.DeleteAsync(po => po.ProductId == id);
            await _productRepository.DeleteAsync(new Product { Id = id });
            return Accepted();
        }
        #endregion

        [HttpGet]
        [Route("{id}/options")]
        public async Task<IActionResult> GetAllOptionsAsync(Guid id)
        {
            Guard.Against.NullGuid(id, nameof(id));
            IEnumerable<ProductOption> allProductOptions = await _productOptionsRepository.ListAsync(po => po.ProductId == id);
            var productOptions = new ProductOptions(allProductOptions);
            return Ok(productOptions);
        }

        [Route("{id}/options/{optionId}")]
        [HttpGet]
        public async Task<IActionResult> GetProductOptionAsync(Guid id, Guid optionId)
        {
            Guard.Against.NullGuid(id, nameof(id));
            Guard.Against.NullGuid(optionId, nameof(optionId));

            var productOption = await _productOptionsRepository.ListAsync(po => po.Id == optionId && po.ProductId == id);
            if (productOption == null)
            {
                return NotFound();
            }
            return Ok(productOption);
        }

        [HttpPost]
        [Route("{id}/options")]
        public async Task<IActionResult> CreateOptionAsync(Guid id, NewProductOption productOption)
        {
            Guard.Against.NullGuid(id, nameof(id));

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
                return BadRequest(ModelState);
            }

            // Can use AutoMapper normally but ignoring for this
            var productOptionModel = new ProductOption
            {
                ProductId = id,
                Name = productOption.Name,
                Description = productOption.Description,
            };
            var savedProductOption = await _productOptionsRepository.AddAsync(productOptionModel);
            return CreatedAtAction(nameof(GetProductOptionAsync), new { id = savedProductOption.ProductId, optionId = savedProductOption.Id  }, savedProductOption);
        }

        [HttpPut]
        [Route("{id}/options/{optionId}")]
        public async Task<IActionResult> UpdateOptionAsync(Guid id, Guid optionId, UpdateProductOption productOption)
        {
            Guard.Against.NullGuid(id, nameof(id));
            Guard.Against.NullGuid(optionId, nameof(optionId));

            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage));
                return BadRequest(ModelState);
            }

            // Can use AutoMapper normally but ignoring for this
            var productOptionModel = new ProductOption
            {
                Id = optionId,
                ProductId = id,
                Name = productOption.Name,
                Description = productOption.Description,
            };
            await _productOptionsRepository.UpdateAsync(productOptionModel);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}/options/{optionId}")]
        public async Task<IActionResult> DeleteOptionAsync(Guid id, Guid optionId)
        {
            Guard.Against.NullGuid(id, nameof(id));
            Guard.Against.NullGuid(optionId, nameof(optionId));
            await _productOptionsRepository.DeleteAsync(new ProductOption { Id = optionId });
            return Accepted();
        }
    }
}