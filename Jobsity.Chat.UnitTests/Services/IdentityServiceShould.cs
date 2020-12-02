using FluentAssertions;
using Jobsity.Chat.Contracts.DTOs;
using Jobsity.Chat.Contracts.Interfaces;
using Jobsity.Chat.Service.IdentityService;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Jobsity.Chat.UnitTests.Services
{
    public class IdentityServiceShould
    {
        private readonly IIdentityService _identityService;
        private readonly Mock<IIdentityRepository> _identityRepositoryMock;

        public IdentityServiceShould()
        {
            _identityRepositoryMock = new Mock<IIdentityRepository>();
            _identityService = new IdentityService(_identityRepositoryMock.Object);
        }

        [Fact]
        public async Task GetUserById()
        {
            // Arrange
            var expectedUserId = Guid.NewGuid().ToString();
            var expectedEmail = "test@mail.com";
            var expectedUserDto = new UserDto() { Id = expectedUserId, Email = expectedEmail, UserName = expectedEmail };
            _identityRepositoryMock.Setup(s => s.GetUserAsync(It.IsAny<string>())).ReturnsAsync(expectedUserDto);

            // Act
            var act = await _identityService.GetUserAsync(expectedUserId);

            // Assert
            act.Should().NotBeNull();
            act.Id.Should().Be(expectedUserId);
            act.Email.Should().Be(expectedEmail);
            act.UserName.Should().Be(expectedEmail);
        }
    }
}
