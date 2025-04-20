using Xunit;
using Moq;
using FluentAssertions;
using Api.FunctionApp.DataContext;
using bookstore.storeBackNet.Repositories;
using ConsultorioNet.Models.Request;
using ConsultorioNet.Models.Response;
using System.Data;
using Dapper;
using bookstore.storeBackNet.DataContext;

namespace Libropedia.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<IDapperContext> _contextMock;
        private readonly Mock<IDapperWrapper> _wrapperMock;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _contextMock = new Mock<IDapperContext>();
             _wrapperMock = new Mock<IDapperWrapper>();
            _categoryService = new CategoryService(_contextMock.Object, _wrapperMock.Object);
        }

        [Fact]
        public async Task FilterCategory_ShouldReturnMatchingCategories()
        {
            // Arrange
            var request = new FilterSearchRequest
            {
                search = "fiction",
                limit = 10
            };

            var expectedCategories = new List<FilterResponse>
            {
                new FilterResponse
                {
                    Id = 1,
                    Value = "Science Fiction"
                },
                new FilterResponse
                {
                    Id = 2,
                    Value = "Fiction"
                }
            };

    var connection = new Mock<IDbConnection>().Object;
          _contextMock.Setup(x => x.CreateConnection())
        .Returns(connection);

   _wrapperMock.Setup(x => x.QueryAsync<FilterResponse>(
    connection,
    It.IsAny<string>(),
    It.IsAny<object>(),
   
    null)) 
    .ReturnsAsync(expectedCategories);

            // Act
            var result = await _categoryService.FilterCategory(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Value == "Science Fiction");
            result.Should().Contain(x => x.Value == "Fiction");
        }

        [Fact]
        public async Task FilterCategory_WithEmptySearch_ShouldReturnAllCategories()
        {
            // Arrange
            var request = new FilterSearchRequest
            {
                search = "",
                limit = 5
            };

            var expectedCategories = new List<FilterResponse>
            {
                new FilterResponse { Id = 1, Value = "Fiction" },
                new FilterResponse { Id = 2, Value = "Non-Fiction" },
                new FilterResponse { Id = 3, Value = "Science" },
                new FilterResponse { Id = 4, Value = "History" },
                new FilterResponse { Id = 5, Value = "Technology" }
            };

           var connection = new Mock<IDbConnection>().Object;
          _contextMock.Setup(x => x.CreateConnection())
        .Returns(connection);

_wrapperMock.Setup(x => x.QueryAsync<FilterResponse>(
    connection,
    It.IsAny<string>(),
    It.IsAny<object>(),
   
    null)) 
    .ReturnsAsync(expectedCategories);
            // Act
            var result = await _categoryService.FilterCategory(request);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(5);
        }
    }
}
