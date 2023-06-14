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
    public class GetByIdTest
    {
        [TestMethod]
        public async Task GetUserById_ReturnsUser_ThereIsDataInDb()
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

            // Use another instance of the context to test our method.
            using (var context = new PersonInfoDbContext(options))
            {
                var controller = new UserController(context);
                var result = await controller.GetUserByIdAsync(1);

                User user = null;

                if (result.Result is OkObjectResult okResult)
                {
                    user = okResult.Value as User;

                }

                Assert.IsNotNull(user);
                Assert.AreEqual("Alice", user.FirstName);
                Assert.AreEqual(1, user.Id);
            }
        }

        [TestMethod]
        public async Task GetUserById_ReturnsNotFound_ThereIsNoSuchUserInDb()
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

            // Use another instance of the context to test our method.
            using (var context = new PersonInfoDbContext(options))
            {
                var controller = new UserController(context);
                var result = await controller.GetUserByIdAsync(3);

                Assert.IsTrue(result.Result is NotFoundResult);
            }
        }

        [TestMethod]
        public async Task GetUserById_ReturnsNotFound_EmptyDb()
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
                var result = await controller.GetUserByIdAsync(1);

                Assert.IsTrue(result.Result is NotFoundResult);
            }
        }

        [TestMethod]
        public async Task GetUserById_Returns400Error_InValidId()
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

            // Use another instance of the context to test our method.
            using (var context = new PersonInfoDbContext(options))
            {
                var controller = new UserController(context);
                var result = await controller.GetUserByIdAsync(-1);

                Assert.IsTrue(result.Result is ObjectResult res && res.StatusCode == 400);
            }
        }
    }
}
