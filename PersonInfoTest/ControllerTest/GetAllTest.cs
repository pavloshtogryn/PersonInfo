using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonInfo.Controllers;
using PersonInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfoTest.ControllerTest
{
    [TestClass]
    public class GetAllTest
    {
        List<User> usersListFromDb = new List<User>()
        {
            new User() {FirstName = "Sophia", LastName = "Smith", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 },
            new User() {FirstName = "Liam", LastName = "Johnson", DateOfBirth = new DateTime(1885, 05, 24), Id = 2 },
            new User() {FirstName = "Olivia", LastName = "Williams", DateOfBirth = new DateTime(2000, 12, 22), Id = 3 }
        };

        [TestMethod]
        public async Task GettAllUsers_ReturnsUsersList_ThereIsDataInDb()
        {
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(usersListFromDb);

            var controller = new UserController(mockRepository.Object);

            ActionResult<IEnumerable<User>> actionResult = await controller.GetAllUsersAsync();

            List<User> users = null;

            if (actionResult.Result is OkObjectResult okResult)
            {
                users = okResult.Value as List<User>;

            }

            Assert.IsNotNull(users);
            Assert.AreEqual(usersListFromDb, users);
        }

        [TestMethod]
        public async Task GettAllUsers_ReturnsNotFound_EmptyListFromDb()
        {
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(new List<User>() { });

            var controller = new UserController(mockRepository.Object);

            ActionResult<IEnumerable<User>> actionResult = await controller.GetAllUsersAsync();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task GettAllUsers_Returns500Error_DbException()
        {
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.GetAllUsersAsync()).Throws(new Exception("Test exception"));

            var controller = new UserController(mockRepository.Object);

            ActionResult<IEnumerable<User>> actionResult = await controller.GetAllUsersAsync();

            Assert.IsTrue(actionResult.Result is ObjectResult result && result.StatusCode == 500);
        }

        [TestMethod]
        public async Task GettAllUsers_ReturnsNotFound_NullFromDb()
        {
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync((List<User>)null);

            var controller = new UserController(mockRepository.Object);

            ActionResult<IEnumerable<User>> actionResult = await controller.GetAllUsersAsync();

            Assert.IsTrue(actionResult.Result is NotFoundResult);
        }
    }
}
