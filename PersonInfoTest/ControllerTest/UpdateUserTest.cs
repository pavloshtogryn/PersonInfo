using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonInfo;
using PersonInfo.Controllers;
using PersonInfo.Models;

namespace PersonInfoTest.ControllerTest
{
    [TestClass]
    public class UpdateUserTest
    {
        private Mock<PersonInfoDbContext> _mockContext;
        private IMemoryCache _memoryCache;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockContext = new Mock<PersonInfoDbContext>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsBadRequest_UserNull()
        {
            var controller = new UserController(_mockContext.Object, _memoryCache);

            var result = await controller.UpdateUserAsync(null);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsBadRequest_UserEmptyName()
        {
            var controller = new UserController(_mockContext.Object, _memoryCache);

            User user = new User() { FirstName = "", LastName = "Smith", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var result = await controller.UpdateUserAsync(user);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsBadRequest_UserEmptyLastName()
        {
            var controller = new UserController(_mockContext.Object, _memoryCache);

            User user = new User() { FirstName = "gdhjgh", LastName = "", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var result = await controller.UpdateUserAsync(user);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsBadRequest_UserNullName()
        {
            var controller = new UserController(_mockContext.Object, _memoryCache);

            User user = new User() { LastName = "dfghgh", DateOfBirth = new DateTime(1990, 02, 12), Id = 1 };

            var result = await controller.UpdateUserAsync(user);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsBadRequest_UserInvalidDateOfBirth()
        {
            var controller = new UserController(_mockContext.Object, _memoryCache);

            User user = new User() { FirstName = "ergedg", LastName = "sdfsdf", DateOfBirth = new DateTime(0001, 01, 01), Id = 1 };

            var result = await controller.UpdateUserAsync(user);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task UpdateUser_ReturnsOk_ValidUser()
        {
            var options = new DbContextOptionsBuilder<PersonInfoDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new PersonInfoDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Users.Add(new User { Id = 1, FirstName = "Alice", LastName = "fghfg", DateOfBirth = DateTime.UtcNow });
                context.Users.Add(new User { Id = 2, FirstName = "Bob", LastName = "gfdhg", DateOfBirth = DateTime.UtcNow });
                context.SaveChanges();
            }

            string firstNameToUpdate = "Sofia";
            string lastNameToUpdate = "Smith";

            // Use another instance of the context to test our method.
            using (var context = new PersonInfoDbContext(options))
            {
                var controller = new UserController(context, _memoryCache);
                User userToUpdate = new User() { FirstName = firstNameToUpdate, LastName = lastNameToUpdate, DateOfBirth = new DateTime(2000, 02, 12), Id = 1 };

                var result = await controller.UpdateUserAsync(userToUpdate);

                Assert.IsInstanceOfType(result, typeof(OkResult));
            }
        }
    }
}
