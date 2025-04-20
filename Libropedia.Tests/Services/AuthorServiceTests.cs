using Xunit;
using Moq;
using FluentAssertions;
using Api.FunctionApp.DataContext;
using bookstore.storeBackNet.Repositories;
using ConsultorioNet.Models.Request;
using ConsultorioNet.Models.Response;
using System.Data;
using Dapper;

namespace Libropedia.Tests.Services
{
    public class AuthorServiceTests
    {
        private readonly Mock<DapperContext> _contextMock;
        private readonly AuthorService _authorService;

        public AuthorServiceTests()
        {
            _contextMock = new Mock<DapperContext>();
            _authorService = new AuthorService(_contextMock.Object);
        }

        [Fact]
        public async Task FilterAuthors_ShouldReturnMatchingAuthors()
        {
            // Arrange
            var request = new FilterSearchRequest
            {
                search = "John",
                limit = 10
            };

            var expectedAuthors = new List<FilterResponse>
            {
                new FilterResponse
                {
                    Id = 1,
                    Value = "John Smith"
                },
                new FilterResponse
                {
                    Id = 2,
                    Value = "John Doe"
                }
            };

            var connection = new Mock<IDbConnection>();
            _contextMock.Setup(x => x.CreateConnection())
                .Returns(connection.Object);

            connection.Setup(x => x.QueryAsync<FilterResponse>(
                It.IsAny<string>(),
                It.IsAny<object>()))
                .ReturnsAsync(expectedAuthors);

            // Act
            var result = await _authorService.FilterAuthors(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Value.Contains("John"));
        }

        [Theory]
        [InlineData("", 5)]
        [InlineData(null, 10)]
        public async Task FilterAuthors_WithEmptyOrNullSearch_ShouldReturnAuthors(string searchTerm, int limit)
        {
            // Arrange
            var request = new FilterSearchRequest
            {
                search = searchTerm,
                limit = limit
            };

            var expectedAuthors = new List<FilterResponse>
            {
                new FilterResponse { Id = 1, Value = "George Orwell" },
                new FilterResponse { Id = 2, Value = "Jane Austen" },
                new FilterResponse { Id = 3, Value = "Ernest Hemingway" }
            };

            var connection = new Mock<IDbConnection>();
            _contextMock.Setup(x => x.CreateConnection())
                .Returns(connection.Object);

            connection.Setup(x => x.QueryAsync<FilterResponse>(
                It.IsAny<string>(),
                It.IsAny<object>()))
                .ReturnsAsync(expectedAuthors);

            // Act
            var result = await _authorService.FilterAuthors(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCountLessOrEqualTo(limit);
        }
    }
}
