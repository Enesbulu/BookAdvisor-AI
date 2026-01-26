using BookAdvisor.Application.DTOs.Auth;
using BookAdvisor.Infrastructure.Identity;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace BookAdvisor.UnitTests.Infrastructure.Identity
{
    public class IdentityServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly IdentityService _identityService;

        public IdentityServiceTests()
        {
            _mockUserManager = MockUserManager();
            _mockConfiguration = new Mock<IConfiguration>();

            _mockConfiguration.Setup(c => c.GetSection("JwtSettings")["Secret"]).Returns("Bu_Cok_Gizli_Bir_Test_Anahtaridir_123456");
            _mockConfiguration.Setup(c => c.GetSection("JwtSettings")["Issuer"]).Returns("TestIssuer");
            _mockConfiguration.Setup(c => c.GetSection("JwtSettings")["Audience"]).Returns("TestAudience");

            _identityService = new IdentityService(_mockUserManager.Object, _mockConfiguration.Object);
        }


        [Fact]
        public async Task LoginAsync_Should_Return_Success_And_Token_When_Credentials_Are_Valid()
        {
            var user = new ApplicationUser
            {
                Id = "user-1",
                Email = "test@test.com",
                UserName = "test@test.com"
            };
            var request = new LoginRequest(Email: "test@test.com", Password: "Password123!");

            _mockUserManager.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
            _mockUserManager.Setup(x => x.CheckPasswordAsync(user: user, password: request.Password)).ReturnsAsync(true);

            var result = await _identityService.LoginAsync(request);

            result.Success.Should().BeTrue();
            result.Token.Should().NotBeNullOrEmpty();
            result.UserId.Should().Be("user-1");
        }

        [Fact]
        public async Task LoginAsync_Should_Return_Fail_When_User_Not_Found()
        {
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
            var result = await _identityService.LoginAsync(new LoginRequest(Email: "yok@yok.com", Password: "123"));

            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Contain("User not found");
        }



        private static Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        }
    }
}
