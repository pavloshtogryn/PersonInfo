using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonInfo.Controllers;
using PersonInfo.Models;

namespace PersonInfoTest.ControllerTest
{
    [TestClass]
    public class UpdateUserTest
    {
        [TestMethod]
        public async Task UpdateUser_ReturnsOk_ValidUser()
        {
            User validUser = new User() { FirstName = "Sophia", LastName = "Smith", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.UpdateAsync(validUser)).Returns(Task.CompletedTask);

            var controller = new UserController(mockRepository.Object);

            ActionResult actionResult = await controller.UpdateUserAsync(validUser);

            Assert.IsTrue(actionResult is OkResult);
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsBadRequest_UserEmptyName()
        {
            User user = new User() { FirstName = "", LastName = "Smith", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.UpdateAsync(user)).Returns(Task.CompletedTask);

            var controller = new UserController(mockRepository.Object);

            ActionResult actionResult = await controller.UpdateUserAsync(user);

            Assert.IsTrue(actionResult is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsBadRequest_UserEmptyLastName()
        {
            User user = new User() { FirstName = "dsfgdfg", LastName = "", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.UpdateAsync(user)).Returns(Task.CompletedTask);

            var controller = new UserController(mockRepository.Object);

            ActionResult actionResult = await controller.UpdateUserAsync(user);

            Assert.IsTrue(actionResult is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsBadRequest_UserNullName()
        {
            User user = new User() { LastName = "fghdfgh", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.UpdateAsync(user)).Returns(Task.CompletedTask);

            var controller = new UserController(mockRepository.Object);

            ActionResult actionResult = await controller.UpdateUserAsync(user);

            Assert.IsTrue(actionResult is BadRequestObjectResult);
        }


        [TestMethod]
        public async Task UpdateUser_ReturnsBadRequest_UserInvalidDateOfBirth()
        {
            User user = new User() { FirstName = "dsfgdfg", LastName = "dfsgdfg", DateOfBirth = new DateTime(0001, 01, 01), Id = 1 };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.UpdateAsync(user)).Returns(Task.CompletedTask);

            var controller = new UserController(mockRepository.Object);

            ActionResult actionResult = await controller.UpdateUserAsync(user);

            Assert.IsTrue(actionResult is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task UpdateUser_Returns500Code_TaskException()
        {
            User validUser = new User() { FirstName = "Sophia", LastName = "Smith", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.UpdateAsync(validUser)).Returns(Task.FromException(new Exception("Exception")));

            var controller = new UserController(mockRepository.Object);

            ActionResult actionResult = await controller.UpdateUserAsync(validUser);

            Assert.IsTrue(actionResult is ObjectResult result && result.StatusCode == 500);
        }

        [TestMethod]
        public async Task UpdateUser_Returns500Code_DbException()
        {
            User validUser = new User() { FirstName = "Sophia", LastName = "Smith", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.UpdateAsync(validUser)).Throws(new Exception("Exception"));

            var controller = new UserController(mockRepository.Object);

            ActionResult actionResult = await controller.UpdateUserAsync(validUser);

            Assert.IsTrue(actionResult is ObjectResult result && result.StatusCode == 500);
        }
    }
}
