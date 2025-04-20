using Xunit;
using Moq;
using FluentAssertions;
using Api.FunctionApp.DataContext;
using bookstore.storeBackNet.Repositories;
using bookstore.storeBackNet.Models.Request;
using bookstore.storeBackNet.Models.Response;
using System.Data;
using Dapper;
using bookstore.storeBackNet.DataContext;

namespace Libropedia.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IDapperContext> _contextMock;
        private readonly Mock<IDapperWrapper> _wrapperMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _contextMock = new Mock<IDapperContext>();
            _wrapperMock = new Mock<IDapperWrapper>();
            _bookService = new BookService(_contextMock.Object, _wrapperMock.Object);
        }

       

        [Fact]
        public async Task GetBookTopQualifications_ShouldReturnTopBooks()
        {
            // Arrange
            var limit = 5;
            var expectedBooks = new List<BookResponse>
            {
                new BookResponse
                {
                    IdLibro = 1,
                    Titulo = "Top Book",
                    Rating = "5.0",
                    TotalRating = "100"
                }
            };

               var connection = new Mock<IDbConnection>().Object;
          _contextMock.Setup(x => x.CreateConnection())
        .Returns(connection);

            _wrapperMock.Setup(x => x.QueryAsync<BookResponse>(
                connection,
                It.IsAny<string>(),
                It.IsAny<object>(),
         
                CommandType.StoredProcedure))
                .ReturnsAsync(expectedBooks);

            // Act
            var result = await _bookService.GetBookTopQualifications(limit);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Titulo.Should().Be("Top Book");
            result.First().Rating.Should().Be("5.0");
        }

     
    }
}
