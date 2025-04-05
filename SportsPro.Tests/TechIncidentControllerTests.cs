using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SportsPro.Controllers;
using SportsPro.Models;
using SportsPro.Models.DataLayer;
using System.Collections.Generic;

namespace SportsPro.Tests
{
    public class TechIncidentControllerTests
    {
        private Mock<IRepository<Technician>> _technicianRepoMock;
        private Mock<IRepository<Incident>> _incidentRepoMock;
        private Mock<IHttpContextAccessor> _httpContextMock;
        private Mock<ISession> _sessionMock;
        private TechIncidentController _controller;

        public TechIncidentControllerTests()
        {
            _technicianRepoMock = new Mock<IRepository<Technician>>();
            _incidentRepoMock = new Mock<IRepository<Incident>>();
            _httpContextMock = new Mock<IHttpContextAccessor>();
            _sessionMock = new Mock<ISession>();

            var httpContext = new DefaultHttpContext();
            httpContext.Session = _sessionMock.Object; 

            _httpContextMock.Setup(a => a.HttpContext).Returns(httpContext);

            _controller = new TechIncidentController(
                _technicianRepoMock.Object,
                _incidentRepoMock.Object,
                _httpContextMock.Object);
        }

        [Fact]
        public void Index_ModelIsATechnicianObject()
        {
            var controller = new TechIncidentController(
                new Mock<IRepository<Technician>>().Object,
                new Mock<IRepository<Incident>>().Object,
                new Mock<IHttpContextAccessor> { DefaultValue = DefaultValue.Mock }.Object
            );

            var model = controller.Index().ViewData.Model;

            Assert.IsType<Technician>(model);
        }

        [Fact]
        public void List_GET_ReturnsAViewResult()
        {
            _technicianRepoMock.Setup(repo => repo.Get(1))
                               .Returns(new Technician { TechnicianID = 1, Name = "Test Tech" });

            var result = _controller.List(1) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void List_POST_ReturnsARedirectToActionResult()
        {
            var technician = new Technician { TechnicianID = 1, Name = "Test Tech" };

            _technicianRepoMock.Setup(repo => repo.Get(1)).Returns(technician);

            _technicianRepoMock.Setup(repo => repo.Save());
                                           
            var result = _controller.List(technician) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("List", result.ActionName);
        }

    }
}