using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.Tests.EfRepository
{
    public class DeleteAsyncTests
    {
        [Fact]
        public async Task Should_Delete_Entity()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProductContext>()
           .UseInMemoryDatabase(databaseName: "Should_Delete_Entity")
           .Options;

            var productEntities = new List<Product>();
            var productA = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product1",
                Description = "Prod 1",
                Price = 10,
                DeliveryPrice = 5
            };
            productEntities.Add(productA);
            productEntities.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product2",
                Description = "Prod 2",
                Price = 20,
                DeliveryPrice = 10
            });

            // Insert seed data into the database using one instance of the context
            using (var context = new ProductContext(options))
            {
                context.Product.AddRange(productEntities);
                await context.SaveChangesAsync();
                // Act
                var efRepo = new EfRepository<Product>(context);
                await efRepo.DeleteAsync(productA);
            }

            // Use a clean instance of the context to run the test
            using (var context = new ProductContext(options))
            {
                // Assert
                Assert.Null(await context.Product.FindAsync(productA.Id));
                Assert.True(await context.Product.CountAsync() == 1);
                Assert.True(await context.ProductOption.CountAsync() == 0);
            }
        }

        [Fact]
        public async Task Should_Delete_Using_Filter()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProductContext>()
           .UseInMemoryDatabase(databaseName: "Should_Delete_Using_Filter")
           .Options;

            var productEntities = new List<Product>();
            var productA = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product1",
                Description = "Prod 1",
                Price = 10,
                DeliveryPrice = 5
            };
            productEntities.Add(productA);
            productEntities.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product2",
                Description = "Prod 2",
                Price = 20,
                DeliveryPrice = 10
            });

            // Insert seed data into the database using one instance of the context
            using (var context = new ProductContext(options))
            {
                context.Product.AddRange(productEntities);
                await context.SaveChangesAsync();
                // Act
                var efRepo = new EfRepository<Product>(context);
                await efRepo.DeleteAsync(p => p.Name == productA.Name);
            }

            // Use a clean instance of the context to run the test
            using (var context = new ProductContext(options))
            {
                // Assert
                Assert.Null(await context.Product.FindAsync(productA.Id));
                Assert.True(await context.Product.CountAsync() == 1);
                Assert.True(await context.ProductOption.CountAsync() == 0);
            }
        }
    }
}
