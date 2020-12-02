using AutoMapper;
using FluentAssertions;
using Jobsity.Chat.UI.Infrastructure;
using System;
using Xunit;

namespace Jobsity.Chat.UnitTests.Infrastructure
{
    public class MappingProfileShould
    {
        [Fact]
        public void BeValid()
        {
            // Arrange
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            IMapper mapper = config.CreateMapper();

            // Act
            Action act = () => mapper.ConfigurationProvider.AssertConfigurationIsValid();

            // Assert
            act.Should().NotThrow();
        }
    }
}
