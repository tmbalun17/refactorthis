using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.Tests.EfRepository
{

    public class AddAsyncTests
    {
        [Fact]
        public async Task Should_Save_Entity()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProductContext>()
           .UseInMemoryDatabase(databaseName: "Should_Save_Entity")
           .Options;

            var productEntities = new List<Product>();
            productEntities.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product1",
                Description = "Prod 1",
                Price = 10,
                DeliveryPrice = 5
            });
            productEntities.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product2",
                Description = "Prod 2",
                Price = 20,
                DeliveryPrice = 10
            });

            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product3",
                Description = "Prod 3",
                Price = 30,
                DeliveryPrice = 15
            };

            // Insert seed data into the database using one instance of the context
            using (var context = new ProductContext(options))
            {
                context.Product.AddRange(productEntities);
                await context.SaveChangesAsync();
            }

            // Use a clean instance of the context to run the test
            using (var context = new ProductContext(options))
            {
                // Act
                var efRepo = new EfRepository<Product>(context);
                var savedProduct = await efRepo.AddAsync(newProduct);
                // Assert
                Assert.NotNull(savedProduct);
                Assert.True(savedProduct.Id == newProduct.Id);
                Assert.True(await context.Product.CountAsync() == 3);
                Assert.True(await context.ProductOption.CountAsync() == 0);
            }
        }
    }
}
