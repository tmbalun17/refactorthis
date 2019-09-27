using Api.Controllers;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests.ProductController
{
    public class GetAllAsync
    {
        [Fact]
        public async Task Should_Return_All_Products()
        {
            //Arrange 
            var entities = new List<Product>();
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
                Name = "Product2",
                Description = "B Prod 2",
                Price = 30,
                DeliveryPrice = 20
            };

            entities.Add(productA);
            entities.Add(productB);
            IReadOnlyList<Product> productEntities = entities;
            var prodRepoMock = new Mock<IProductRepository>();
            var prodOptionRepoMock = new Mock<IProductOptionRepository>();
            prodRepoMock.Setup(pm => pm.ListAllAsync()).Returns(Task.FromResult(productEntities)).Verifiable();

            //Act
            var controller = new ProductV1Controller(prodRepoMock.Object, prodOptionRepoMock.Object);
            var actionResult = await controller.GetAllAsync(null);

            //Assert
            Mock.Verify(prodRepoMock);
            actionResult.As<IActionResult>().Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var products = (Products)actionResult.As<OkObjectResult>().Value;
            Assert.NotNull(products);
            Assert.True(products.Items.Count() == 2);
            Assert.Collection(products.Items,
                              item => Assert.Contains("Product1", item.Name),
                              item => Assert.Contains("Product2", item.Name));

        }

        [Fact]
        public async Task Should_Return_Products_Matching_Name()
        {
            //Arrange 
            var entities = new List<Product>();

            var productB = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product2",
                Description = "B Prod 2",
                Price = 30,
                DeliveryPrice = 20
            };

            entities.Add(productB);
            IReadOnlyList<Product> productEntities = entities;
            var prodRepoMock = new Mock<IProductRepository>();
            var prodOptionRepoMock = new Mock<IProductOptionRepository>();
            prodRepoMock.Setup(pm => pm.ListAsync(It.IsAny<Expression<Func<Product, bool>>>(), null, null))
                .Returns(Task.FromResult(productEntities)).Verifiable();

            //Act
            var controller = new ProductV1Controller(prodRepoMock.Object, prodOptionRepoMock.Object);
            var actionResult = await controller.GetAllAsync(productB.Name);

            //Assert
            Mock.Verify(prodRepoMock);
            actionResult.As<IActionResult>().Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var products = (Products)actionResult.As<OkObjectResult>().Value;
            Assert.NotNull(products);
            Assert.True(products.Items.Count() == 1);
            Assert.True(products.Items.First().Id == productB.Id);
        }
    }

    public class GetProductAsync
    {
        [Fact]
        public async Task Should_Return_Product()
        {
            //Arrange 
            var productA = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product1",
                Description = "A Prod 1",
                Price = 10,
                DeliveryPrice = 5
            };

            var prodRepoMock = new Mock<IProductRepository>();
            var prodOptionRepoMock = new Mock<IProductOptionRepository>();
            prodRepoMock.Setup(pm => pm.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(productA)).Verifiable();

            //Act
            var controller = new ProductV1Controller(prodRepoMock.Object, prodOptionRepoMock.Object);
            var actionResult = await controller.GetProductAsync(productA.Id);

            //Assert
            Mock.Verify(prodRepoMock);
            actionResult.As<IActionResult>().Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var product = (Product)actionResult.As<OkObjectResult>().Value;
            Assert.NotNull(product);
            Assert.True(product.Id == productA.Id);
        }

        [Fact]
        public async Task Should_Return_NotFound()
        {
            //Arrange 
            var prodRepoMock = new Mock<IProductRepository>();
            var prodOptionRepoMock = new Mock<IProductOptionRepository>();
            prodRepoMock.Setup(pm => pm.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult((Product)null)).Verifiable();

            //Act
            var controller = new ProductV1Controller(prodRepoMock.Object, prodOptionRepoMock.Object);
            var actionResult = await controller.GetProductAsync(Guid.NewGuid());

            //Assert
            Mock.Verify(prodRepoMock);
            actionResult.As<IActionResult>().Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
