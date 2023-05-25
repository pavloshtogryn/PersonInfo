using PersonInfo;
using PersonInfo.Controllers;
using PersonInfo.Models;
using Moq.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using PersonInfo;

namespace PersonInfoTest.ControllerTest
{

    [TestClass]
    public class GetAllTest
    {
        [TestMethod]
        public async Task GettAllUsers_ReturnsUsersList_ThereIsDataInDb()
        {
            var usersListFromDb = new List<User>()
            {
                new User() {FirstName = "Sophia", LastName = "Smith", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 },
                new User() {FirstName = "Liam", LastName = "Johnson", DateOfBirth = new DateTime(1885, 05, 24), Id = 2 },
                new User() {FirstName = "Olivia", LastName = "Williams", DateOfBirth = new DateTime(2000, 12, 22), Id = 3 }
            }.AsQueryable();

            var mockSet = usersListFromDb.AsDbSetMock();

            var mockContext = new Mock<PersonInfoDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var controller = new UserController(mockContext.Object);
            ActionResult<IEnumerable<User>> actionResult = await controller.GetAllUsersAsync();

            List<User> users = null;

            if (actionResult.Result is OkObjectResult okResult)
            {
                users = okResult.Value as List<User>;
            }

            Assert.IsNotNull(users);
            Assert.AreEqual(usersListFromDb.Count(), users.Count);
        }

        
        [TestMethod]
        public async Task GettAllUsers_ReturnsNotFound_EmptyListFromDb()
        {
            var usersListFromDb = new List<User>()
            { }.AsQueryable();

            var mockSet = usersListFromDb.AsDbSetMock();

            var mockContext = new Mock<PersonInfoDbContext>();
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            var controller = new UserController(mockContext.Object);
            ActionResult<IEnumerable<User>> actionResult = await controller.GetAllUsersAsync();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task GettAllUsers_Returns500Error_DbException()
        {

            var mockContext = new Mock<PersonInfoDbContext>();
            mockContext.Setup(c => c.Users).Throws(new Exception("Test exception"));

            var controller = new UserController(mockContext.Object);
            ActionResult<IEnumerable<User>> actionResult = await controller.GetAllUsersAsync();

            Assert.IsTrue(actionResult.Result is ObjectResult result && result.StatusCode == 500);
        }
    }
}
