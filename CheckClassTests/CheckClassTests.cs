using Moq;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using SportsPro.Models;
using SportsPro.Controllers;
using SportsPro.Models.DataLayer;
namespace CheckClassTests
{
    public class CheckClassTests
    {
        [Fact]
        public void EmailExists_ReturnsAnEmptyStringIfEmailsMissing()
        {
            var rep = new Mock<IRepository<Customer>>() { DefaultValue = DefaultValue.Mock };
        }

        [Fact]
        public void EmailExists_ReturnsAnEmptyStringIfEmailsNew()
        {

        }
        [Fact]
        public void EmailEXists_ReturnsANErrorMessageIfEmailExists()
        {

        }
        [Fact]
        public void EmailExists_ReturnsAString()
        {

        }
        
    }
}