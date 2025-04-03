using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SportsPro.Controllers;
using SportsPro.Models;
using SportsPro.Models.DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SportsPro.Tests
{
    public class ProductControllerTests
    {
        private Mock<IRepository<Product>> _productRepoMock;
        private ProductController _controller;

        public ProductControllerTests()
        {
            _productRepoMock = new Mock<IRepository<Product>>();
            _controller = new ProductController(_productRepoMock.Object);
        }

        [Fact]
        public void Add_GET_ModelsAProductObject()
        {
            var result = _controller.Add() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<Product>(result.Model);
        }

        [Fact]
        public void Add_GET_ValueOfViewBagActionPropertyIsAdd()
        {
            var result = _controller.Add() as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Add", result.ViewData["Mode"]);
        }

        [Fact]
        public void Delete_GET_ModelsAProductObject()
        {
            var product = new Product { ProductID = 1, Name = "Test Product" };
            _productRepoMock.Setup(repo => repo.Get(1)).Returns(product);

            var result = _controller.Delete(1) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(product, result.Model);
        }

        [Fact]
        public void Delete_GET_ReturnsAViewResult()
        {
            var product = new Product { ProductID = 1, Name = "Test Product" };

            _productRepoMock.Setup(repo => repo.Get(1)).Returns(product);

            var result = _controller.Delete(1) as ViewResult;  

            Assert.NotNull(result);  
            Assert.Equal(product, result.Model); 
        }


        [Fact]
        public void Delete_POST_ReturnsARedirectToActionResult()
        {
            var product = new Product { ProductID = 1, Name = "Test Product" };
            _productRepoMock.Setup(repo => repo.Get(1)).Returns(product);  
            _productRepoMock.Setup(repo => repo.Delete(product));         
            _productRepoMock.Setup(repo => repo.Save());                 

            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            _controller.TempData = tempData;

            var result = _controller.Delete(product) as RedirectToActionResult; 

            Assert.NotNull(result);  
            Assert.Equal("List", result.ActionName);  
            Assert.Equal($"{product.Name} was deleted.", tempData["message"]);  
        }



        [Fact]
        public void Edit_GET_ModelsAProductObject()
        {
            var product = new Product { ProductID = 1, Name = "Test Product" };
            _productRepoMock.Setup(repo => repo.Get(1)).Returns(product);

            var result = _controller.Edit(1) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(product, result.Model);
        }

        [Fact]
        public void Edit_GET_ValueOfViewBagActionPropertyIsEdit()
        {
            var product = new Product { ProductID = 1, Name = "Test Product" };

            _productRepoMock.Setup(repo => repo.Get(1)).Returns(product);

            var result = _controller.Edit(1) as ViewResult;  

            Assert.NotNull(result);  
            Assert.Equal("Edit", result.ViewData["Mode"]);  
        }

        [Fact]
        public void List_ModelsACollectionOfProducts()
        {
            var products = new List<Product> { new Product { ProductID = 1, Name = "Product 1" } };
            _productRepoMock.Setup(repo => repo.List(It.IsAny<QueryOptions<Product>>())).Returns(products);

            var result = _controller.List() as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(products, result.Model);
        }

        [Fact]
        public void List_ReturnsAViewResult()
        {
            var result = _controller.List() as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Save_RedirectsToListActionMethodOnSuccess()
        {
            var tempDataMock = new Mock<ITempDataDictionary>();
            _controller.TempData = tempDataMock.Object;

            var product = new Product
            {
                ProductID = 1,
                ProductCode = "P001",
                Name = "Sample Product",
                YearlyPrice = 99.99m,
                ReleaseDate = DateTime.Now
            };

            _productRepoMock.Setup(repo => repo.Insert(It.IsAny<Product>()));
            _productRepoMock.Setup(repo => repo.Save());

            var result = _controller.Add(product) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("List", result.ActionName);  

            tempDataMock.Verify(tempData => tempData["message"], Times.Once, "TempData['message'] was not set.");
        }

        [Fact]
        public void Save_ReturnsRedirectToActionResultIfModelStateIsValid()
        {
            var product = new Product { ProductID = 1, Name = "New Product" };

            _productRepoMock.Setup(repo => repo.Insert(It.IsAny<Product>())).Verifiable();
            _productRepoMock.Setup(repo => repo.Save()).Verifiable();

            _controller.ModelState.Clear(); 

            var tempDataMock = new Mock<ITempDataDictionary>();
            _controller.TempData = tempDataMock.Object;

            var result = _controller.Add(product) as RedirectToActionResult;

            Assert.NotNull(result);  
            Assert.Equal("List", result.ActionName);  

            _productRepoMock.Verify();

            tempDataMock.Verify(tempData => tempData["message"], Times.Once(), "TempData['message'] was not set.");
        }

        [Fact]
        public void Save_ReturnsViewResultIfModelStateIsInvalid()
        {
            var product = new Product { ProductID = 1, Name = "New Product" };
            _controller.ModelState.AddModelError("Name", "Required");

            var result = _controller.Add(product) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}