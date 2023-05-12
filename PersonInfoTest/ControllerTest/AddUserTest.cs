using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonInfo.Controllers;
using PersonInfo.Models;

namespace PersonInfoTest.ControllerTest
{
    [TestClass]
    public class AddUserTest
    {

        [TestMethod]
        public async Task CreateUser_ReturnsId_ValidUser()
        {
            User validUserToCreate = new User() { FirstName = "Sophia", LastName = "Smith", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };
            int createdUserId = 2;

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.CreateAsync(validUserToCreate)).ReturnsAsync(createdUserId);

            var controller = new UserController(mockRepository.Object);

            ActionResult<int> actionResult = await controller.AddUserAsync(validUserToCreate);

            int id = 0;

            if (actionResult.Result is CreatedAtActionResult result && result.Value != null)
            {
                var type = result.Value.GetType();
                var prop = type.GetProperty("id");
                if (prop != null)
                {
                    id = (int)prop.GetValue(result.Value, null);
                }
            }

            Assert.AreEqual(createdUserId, id);
        }

        [TestMethod]
        public async Task CreateUser_ReturnsBadRequest_UserNull()
        {
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<User>())).ReturnsAsync(It.IsAny<int>());

            var controller = new UserController(mockRepository.Object);

            ActionResult<int> actionResult = await controller.AddUserAsync(null);

            Assert.IsTrue(actionResult.Result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task CreateUser_ReturnsBadRequest_UserEnptyName()
        {
            User user = new User() { FirstName = "", LastName = "Smith", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<User>())).ReturnsAsync(It.IsAny<int>());

            var controller = new UserController(mockRepository.Object);


            ActionResult<int> actionResult = await controller.AddUserAsync(user);

            Assert.IsTrue(actionResult.Result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task CreateUser_ReturnsBadRequest_UserEnptyLastName()
        {
            User user = new User() { FirstName = "ergedg", LastName = "", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<User>())).ReturnsAsync(It.IsAny<int>());

            var controller = new UserController(mockRepository.Object);


            ActionResult<int> actionResult = await controller.AddUserAsync(user);

            Assert.IsTrue(actionResult.Result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task CreateUser_ReturnsBadRequest_UserNullName()
        {
            User user = new User() { LastName = "dfgdfg", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<User>())).ReturnsAsync(It.IsAny<int>());

            var controller = new UserController(mockRepository.Object);


            ActionResult<int> actionResult = await controller.AddUserAsync(user);

            Assert.IsTrue(actionResult.Result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task CreateUser_Returns500Error_DbException()
        {
            User user = new User() { FirstName = "ergedg", LastName = "dfgdfgf", DateOfBirth = new DateTime(0001, 02, 12), Id = 1 };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<User>())).Throws(new Exception("Test exception"));

            var controller = new UserController(mockRepository.Object);


            ActionResult<int> actionResult = await controller.AddUserAsync(user);

            Assert.IsTrue(actionResult.Result is ObjectResult result && result.StatusCode == 500);
        }

        [TestMethod]
        public async Task CreateUser_ReturnsBadRequest_UserInvalidDateOfBirth()
        {
            User user = new User() { FirstName = "ergedg", LastName = "sdfsdf", DateOfBirth = new DateTime(0001, 01, 01), Id = 1 };

            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<User>())).ReturnsAsync(It.IsAny<int>());

            var controller = new UserController(mockRepository.Object);


            ActionResult<int> actionResult = await controller.AddUserAsync(user);

            Assert.IsTrue(actionResult.Result is BadRequestObjectResult);
        }

    }
}
