using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonInfo.Controllers;
using PersonInfo.Models;

namespace PersonInfoTest.ControllerTest
{
    [TestClass]
    public class GetByIdTest
    {
        
        User userFromDb = new User() { FirstName = "Sophia", LastName = "Smith", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

        [TestMethod]
        public async Task GetUserById_ReturnsUser_ThereIsDataInDb()
        {
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.GetAsync(userFromDb.Id)).ReturnsAsync(userFromDb);

            var controller = new UserController(mockRepository.Object);

            ActionResult<User> actionResult = await controller.GetUserByIdAsync(userFromDb.Id);

            User user = null;

            if (actionResult.Result is OkObjectResult okResult)
            {
                user = okResult.Value as User;

            }

            Assert.IsNotNull(user);
            Assert.AreEqual(userFromDb, user);
        }

        /*
        [TestMethod]
        public async Task GetUserById_ReturnsNotFound_EmptyUserFromDb()
        {
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.GetAsync(userFromDb.Id)).ReturnsAsync(new User() { });

            var controller = new UserController(mockRepository.Object);

            ActionResult<User> actionResult = await controller.GetUserByIdAsync(userFromDb.Id);

            Assert.IsTrue(actionResult.Result is NotFoundResult);
        }
        [TestMethod]
        public async Task GetUserById_ReturnsNotFound_NullFromDb()
        {
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.GetAsync(userFromDb.Id)).ReturnsAsync((User)null);

            var controller = new UserController(mockRepository.Object);

            ActionResult<User> actionResult = await controller.GetUserByIdAsync(userFromDb.Id);

            Assert.IsTrue(actionResult.Result is NotFoundResult);
        }

        [TestMethod]
        public async Task GetUserById_Returns500Error_DbException()
        {
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.GetAsync(userFromDb.Id)).Throws(new Exception("Test exception"));

            var controller = new UserController(mockRepository.Object);

            ActionResult<User> actionResult = await controller.GetUserByIdAsync(userFromDb.Id);

            Assert.IsTrue(actionResult.Result is ObjectResult result && result.StatusCode == 500);
        }

        [TestMethod]
        public async Task GetUserById_Returns400Error_InValidId()
        {
            var mockRepository = new Mock<IUserRepository>();

            mockRepository.Setup(repo => repo.GetAsync(userFromDb.Id)).ReturnsAsync(new User() { });

            var controller = new UserController(mockRepository.Object);

            ActionResult<User> actionResult = await controller.GetUserByIdAsync(-1);

            Assert.IsTrue(actionResult.Result is ObjectResult result && result.StatusCode == 400);
        }
        */
    }
}
