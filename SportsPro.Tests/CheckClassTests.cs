using Xunit;
using SportsPro.Models.DataLayer;
using Moq;
using SportsPro.Models;

namespace SportsPro.Tests
{
    public class CheckClassTests
    {
        [Fact]
        public void EmailExists_ReturnsAnEmptyStringIfEmailsMissing()
        {
            var mockRepository = new Mock<IRepository<Customer>>();
            mockRepository.Setup(repo => repo.List(It.IsAny<QueryOptions<Customer>>()))
                          .Returns(new List<Customer>());

            var result = Check.EmailExists(mockRepository.Object, "nonexistentemail@example.com", 0);

            Assert.Equal(string.Empty, result); 
        }

        [Fact]
        public void EmailExists_ReturnsAnEmptyStringIfEmailIsNew()
        {
            var mockRepository = new Mock<IRepository<Customer>>(); 
            mockRepository.Setup(repo => repo.List(It.IsAny<QueryOptions<Customer>>()))
                          .Returns(new List<Customer>()); 

            var result = Check.EmailExists(mockRepository.Object, "newemail@example.com", 0);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void EmailExists_ReturnsAnErrorMessageIfEmailExists()
        {
            var mockRepository = new Mock<IRepository<Customer>>(); 

            var existingCustomer = new Customer
            {
                CustomerID = 1,
                Email = "existingemail@example.com"
            };

            mockRepository.Setup(repo => repo.List(It.IsAny<QueryOptions<Customer>>()))
                          .Returns(new List<Customer> { existingCustomer });

            var result = Check.EmailExists(mockRepository.Object, "existingemail@example.com", 0);

            Assert.Equal("Email address already in use.", result);  
        }

        [Fact]
        public void EmailExists_ReturnsAString()
        {
            var mockRepository = new Mock<IRepository<Customer>>();

            var existingCustomer = new Customer
            {
                CustomerID = 1,
                Email = "testemail@example.com"
            };

            mockRepository.Setup(repo => repo.List(It.IsAny<QueryOptions<Customer>>()))
                          .Returns(new List<Customer> { existingCustomer });

            var result = Check.EmailExists(mockRepository.Object, "testemail@example.com", 0); 

            Assert.Equal("Email address already in use.", result);
        }
    }
}