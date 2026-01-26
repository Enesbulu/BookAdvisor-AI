using BookAdvisor.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace BookAdvisor.UnitTests.API.Services
{
    public class CurrentUserServiceTests
    {
        [Fact]

        public void UserId_Should_Return_Value_When_User_Is_Authenticated()
        {
            var userId = "user-123";
            var mocHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId) };
            context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));

            mocHttpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var service = new CurrentUserService(mocHttpContextAccessor.Object);
            var result = service.UserId;
            result.Should().Be(userId);

        }

        [Fact]
        public void UserId_Should_Return_Null_When_HttpContext_Is_Null()
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns((HttpContext)null);

            var service = new CurrentUserService(mockHttpContextAccessor.Object);
            var result = service.UserId;
            result.Should().BeNull();

        }


    }
}
