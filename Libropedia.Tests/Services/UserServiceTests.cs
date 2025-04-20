using Xunit;
using Moq;
using FluentAssertions;
using Api.FunctionApp.DataContext;
using bookstore.storeBackNet.Repositories;
using ConsultorioNet.Models.Request;
using EventManagementSystem.Helpers;
using System.Data;
using Dapper;
using bookstore.storeBackNet.DataContext;

namespace Libropedia.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IDapperContext> _contextMock;
        private readonly JwtSettings _jwtSettings;
        private readonly Mock<IDapperWrapper> _wrapperMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _contextMock = new Mock<IDapperContext>();
            _jwtSettings = new JwtSettings{
                SecretKey = "holaEstoEsUnaKayPrivadaParaTestear",
                TokenExpirationInMinutes = 60
            };
            _wrapperMock = new Mock<IDapperWrapper>();
            _userService = new UserService(_contextMock.Object, _jwtSettings, _wrapperMock.Object);
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

           var connection = new Mock<IDbConnection>().Object;
          _contextMock.Setup(x => x.CreateConnection())
        .Returns(connection);

            _wrapperMock.Setup(x => x.ExecuteAsync(
                connection,
                It.IsAny<string>(),
                It.IsAny<object>(),
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

              var connection = new Mock<IDbConnection>().Object;
          _contextMock.Setup(x => x.CreateConnection())
        .Returns(connection);

            _wrapperMock.Setup(x => x.QueryFirstOrDefaultAsync<ConsultorioNet.Models.Response.UserResponse>(
                connection,
                It.IsAny<string>(),
                It.IsAny<object>(),
                CommandType.StoredProcedure))
                .ReturnsAsync(userResponse);

     

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
