using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonInfo;
using PersonInfo.Controllers;
using PersonInfo.Models;

namespace PersonInfoTest.ControllerTest
{
    [TestClass]
    public class AddUserTest
    {
        private Mock<PersonInfoDbContext> _mockContext;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockContext = new Mock<PersonInfoDbContext>();
        }

        [TestMethod]
        public async Task CreateUser_ReturnsBadRequest_UserNull()
        {
            var controller = new UserController(_mockContext.Object);

            var result = await controller.AddUserAsync(null);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CreateUser_ReturnsBadRequest_UserEnptyName()
        {
            var controller = new UserController(_mockContext.Object);

            User user = new User() { FirstName = "", LastName = "Smith", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var result = await controller.AddUserAsync(user);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CreateUser_ReturnsBadRequest_UserEnptyLastName()
        {
            var controller = new UserController(_mockContext.Object);

            User user = new User() { FirstName = "gdhjgh", LastName = "", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var result = await controller.AddUserAsync(user);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CreateUser_ReturnsBadRequest_UserNullName()
        {
            var controller = new UserController(_mockContext.Object);

            User user = new User() { LastName = "dfghgh", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var result = await controller.AddUserAsync(user);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CreateUser_ReturnsBadRequest_UserInvalidDateOfBirth()
        {
            var controller = new UserController(_mockContext.Object);

            User user = new User() { FirstName = "ergedg", LastName = "sdfsdf", DateOfBirth = new DateTime(0001, 01, 01), Id = 1 };

            var result = await controller.AddUserAsync(user);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task CreateUser_ReturnsId_ValidUser()
        {
            var options = new DbContextOptionsBuilder<PersonInfoDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new PersonInfoDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.SaveChanges();
            }

            // Use another instance of the context to test our method.
            using (var context = new PersonInfoDbContext(options))
            {
                var controller = new UserController(context);
                User validUserToCreate = new User() { FirstName = "Sophia", LastName = "Smith", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

                var result = await controller.AddUserAsync(validUserToCreate);

                Assert.IsInstanceOfType(result.Result, typeof(CreatedAtActionResult));
            }
        }
    }
}
