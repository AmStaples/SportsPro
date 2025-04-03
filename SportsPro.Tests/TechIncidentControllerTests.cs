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
            httpContext.Session = _sessionMock.Object; // Assign the mocked session

            _httpContextMock.Setup(a => a.HttpContext).Returns(httpContext); // Ensure HttpContext is set

            _controller = new TechIncidentController(
                _technicianRepoMock.Object,
                _incidentRepoMock.Object,
                _httpContextMock.Object);
        }

        [Fact]
        public void Index_ModelsATechnicianObject()
        {
            // Arrange: Mock HttpContext and Session
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockSession = new Mock<ISession>();

            // Mock the session behavior (return a value when GetInt32 is called)
            mockSession.Setup(s => s.GetInt32(It.IsAny<string>())).Returns(1); // 1 is the technician ID
            mockHttpContext.Setup(ctx => ctx.Session).Returns(mockSession.Object);
            mockHttpContextAccessor.Setup(acc => acc.HttpContext).Returns(mockHttpContext.Object);

            // Mock the Technician Repository
            var mockTechnicianRepo = new Mock<IRepository<Technician>>();
            var technician = new Technician { TechnicianID = 1, Name = "John Doe", Email = "john.doe@example.com", Phone = "123-456-7890" };

            mockTechnicianRepo.Setup(repo => repo.List(It.IsAny<QueryOptions<Technician>>()))
                              .Returns(new List<Technician> { technician });
            mockTechnicianRepo.Setup(repo => repo.Get(technician.TechnicianID))
                              .Returns(technician);

            // Mock the Incident Repository (You need to initialize this too)
            var mockIncidentRepo = new Mock<IRepository<Incident>>(); // This was missing
            var incident = new Incident { IncidentID = 1, Title = "Test Incident", TechnicianID = 1 }; // You can mock an example incident if needed
            mockIncidentRepo.Setup(repo => repo.List(It.IsAny<QueryOptions<Incident>>()))
                            .Returns(new List<Incident> { incident });

            // Instantiate the controller with the mocked dependencies
            var controller = new TechIncidentController(mockTechnicianRepo.Object, mockIncidentRepo.Object, mockHttpContextAccessor.Object);

            // Act
            var result = controller.Index() as ViewResult;
            var model = result?.Model as Technician;

            // Assert
            Assert.NotNull(model); // Ensure the model is not null
            Assert.Equal("John Doe", model.Name); // Now safe to check Name
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