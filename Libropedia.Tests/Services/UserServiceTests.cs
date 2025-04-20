using Xunit;
using Moq;
using FluentAssertions;
using Api.FunctionApp.DataContext;
using bookstore.storeBackNet.Repositories;
using ConsultorioNet.Models.Request;
using EventManagementSystem.Helpers;
using System.Data;
using Dapper;

namespace Libropedia.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<DapperContext> _contextMock;
        private readonly Mock<JwtSettings> _jwtSettingsMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _contextMock = new Mock<DapperContext>();
            _jwtSettingsMock = new Mock<JwtSettings>();
            _userService = new UserService(_contextMock.Object, _jwtSettingsMock.Object);
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "test@test.com",
                Password = "Test123!",
                Username = "testuser"
            };

            var connection = new Mock<IDbConnection>();
            _contextMock.Setup(x => x.CreateConnection())
                .Returns(connection.Object);

            connection.Setup(x => x.ExecuteAsync(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                CommandType.StoredProcedure))
                .ReturnsAsync(1);

            // Act
            var result = await _userService.RegisterAsync(registerRequest);

            // Assert
            result.Should().NotBeNull();
            result.IsError.Should().BeFalse();
            result.Message.Should().Be("User registered successfully");
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@test.com",
                Password = "Test123!"
            };

            var userResponse = new ConsultorioNet.Models.Response.UserResponse
            {
                Id = 1,
                Email = "test@test.com",
                Username = "testuser",
                Is_active = "1"
            };

            var connection = new Mock<IDbConnection>();
            _contextMock.Setup(x => x.CreateConnection())
                .Returns(connection.Object);

            connection.Setup(x => x.QueryFirstOrDefaultAsync<ConsultorioNet.Models.Response.UserResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                CommandType.StoredProcedure))
                .ReturnsAsync(userResponse);

            _jwtSettingsMock.SetupGet(x => x.SecretKey).Returns("your-secret-key-here");
            _jwtSettingsMock.SetupGet(x => x.TokenExpirationInMinutes).Returns(60);

            // Act
            var result = await _userService.LoginAsync(loginRequest);

            // Assert
            result.Should().NotBeNull();
            result.IsError.Should().BeFalse();
            result.Token.Should().NotBeNullOrEmpty();
            result.User.Should().NotBeNull();
            result.User.Email.Should().Be(loginRequest.Email);
        }
    }
}
