using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Infrastructure.Tests.EfRepository
{
    public class ListAsyncTests
    {
        [Fact]
        public async Task ShouldGet_AllEntites()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProductContext>()
           .UseInMemoryDatabase(databaseName: "ShouldGet_AllEntites")
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
                var allProducts = await efRepo.ListAsync();
                // Assert
                Assert.NotNull(allProducts);
                Assert.True(allProducts.Count == 2);
                Assert.Collection<Product>(allProducts, 
                              item => Assert.Contains("Product1", item.Name),
                              item => Assert.Contains("Product2", item.Name));
            }
        }

        [Fact]
        public async Task Should_List_Using_Filter()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProductContext>()
           .UseInMemoryDatabase(databaseName: "Should_List_Using_Filter")
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
            }

            // Use a clean instance of the context to run the test
            using (var context = new ProductContext(options))
            {
                // Act
                var efRepo = new EfRepository<Product>(context);
                var filteredProducts = await efRepo.ListAsync(p => p.Name == productA.Name);
                // Assert
                Assert.NotNull(filteredProducts);
                Assert.True(filteredProducts.Count == 1);
                Assert.Collection<Product>(filteredProducts, 
                              item => Assert.Contains("Product1", item.Name));
            }
        }

        [Fact]
        public async Task Should_List_With_Filter_Ordered()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProductContext>()
           .UseInMemoryDatabase(databaseName: "Should_List_With_Filter_Ordered")
           .Options;

            var productEntities = new List<Product>();
            var productA = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product1",
                Description = "A Prod 1",
                Price = 10,
                DeliveryPrice = 5
            };

            var productB = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product1",
                Description = "B Prod 1",
                Price = 30,
                DeliveryPrice = 20
            };

            productEntities.Add(productA);
            productEntities.Add(productB);
            productEntities.Add(new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product2",
                Description = "A Prod 2",
                Price = 20,
                DeliveryPrice = 10
            });

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
                var filteredProducts = await efRepo.ListAsync(p => p.Name == productA.Name, po => po.Description);
                // Assert
                Assert.NotNull(filteredProducts);
                Assert.True(filteredProducts.Count == 2);
                Assert.True(filteredProducts.First().Id == productA.Id);
                Assert.True(filteredProducts.Last().Id == productB.Id);
            }
        }

        [Fact]
        public async Task Should_List_In_Desc_Order()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ProductContext>()
           .UseInMemoryDatabase(databaseName: "Should_List_In_Desc_Order")
           .Options;

            var productEntities = new List<Product>();
            var productA = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product1",
                Description = "A Prod 1",
                Price = 10,
                DeliveryPrice = 5
            };

            var productB = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product1",
                Description = "B Prod 1",
                Price = 30,
                DeliveryPrice = 20
            };
            
            var productC = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product2",
                Description = "C Prod 2",
                Price = 20,
                DeliveryPrice = 10
            };

            productEntities.Add(productA);
            productEntities.Add(productB);
            productEntities.Add(productC);

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
                var filteredProducts = await efRepo.ListAsync(orderByDescending: po => po.Description);
                // Assert
                Assert.NotNull(filteredProducts);
                Assert.True(filteredProducts.Count == 3);
                Assert.True(filteredProducts.First().Id == productC.Id);
                Assert.True(filteredProducts.Skip(1).First().Id == productB.Id);
                Assert.True(filteredProducts.Last().Id == productA.Id);
            }
        }
    }
}
