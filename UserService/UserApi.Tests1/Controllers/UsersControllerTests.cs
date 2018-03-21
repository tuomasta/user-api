using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserApi.Controllers;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using UserApi.Repos;
using UserApi.Messaging;
using System;
using System.Linq.Expressions;

namespace UserApi.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        Mock<IUserRepo> _useRepoMock;
        Mock<IMessageBus> _messageBusMock;
        UsersController _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _useRepoMock = new Mock<IUserRepo>();
            _useRepoMock.Setup(b => b.CreateUserAsync(It.IsAny<User>())).Returns<User>(user => Task.FromResult(user));

            _messageBusMock = new Mock<IMessageBus>();
            _messageBusMock.Setup(b => b.SendEventAsync(It.IsAny<IUserEvent>())).Returns(Task.CompletedTask);

             _target = new UsersController(_useRepoMock.Object, _messageBusMock.Object);
        }

        [TestMethod]
        public async Task WHEN_requesting_user_THEN_returns_the_users_from_repository()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedUser = new User() { Id = id };
            _useRepoMock.Setup(r => r.GetUserAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(expectedUser);

            // Act
            var result = (await _target.GetAsync(id)) as OkNegotiatedContentResult<User>;
            var content = result.Content;

            _useRepoMock.Verify(r => r.GetUserAsync(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
            Assert.AreEqual(expectedUser, result?.Content);
        }

        [TestMethod]
        public async Task WHEN_requesting_users_THEN_returns_users_from_repository()
        {
            // Arrange
            var expectedUser = new User() { Id = Guid.NewGuid() };
            _useRepoMock.Setup(r => r.GetUsersAsync(null)).ReturnsAsync(new[] { expectedUser });

            // Act
            var result = (await _target.GetAsync()) as OkNegotiatedContentResult<IEnumerable<User>>;
            var content = result.Content;

            _useRepoMock.Verify(r => r.GetUsersAsync(null), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.Count());
            Assert.AreEqual(expectedUser, result.Content.Single());
        }

        [TestMethod]
        public async Task WHEN_posting_user_without_required_data_THEN_gives_bad_request_result()
        {
            // Arrange
            var expectedUser = new User() { Id = Guid.NewGuid() };

            // Act
            var result = (await _target.Post(expectedUser)) as BadRequestErrorMessageResult;
            Assert.IsNotNull(result?.Message);
            _useRepoMock.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public async Task WHEN_posting_user_with_required_data_THEN_saves_to_repo_AND_sends_an_event_AND_returns_the_user()
        {
            // Arrange
            var expectedUser = new User() { Id = Guid.NewGuid(), Email = "mail", Password = "password" };
           
            // Act
            var result = (await _target.Post(expectedUser)) as CreatedNegotiatedContentResult<User>;
            Assert.IsNotNull(result);
    
            _useRepoMock.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Once);
            _messageBusMock.Verify(r => r.SendEventAsync(It.IsAny<UserCreatedEvent>()), Times.Once);
        }
    }
}
