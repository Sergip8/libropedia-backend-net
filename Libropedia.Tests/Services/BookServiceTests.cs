using Xunit;
using Moq;
using FluentAssertions;
using Api.FunctionApp.DataContext;
using bookstore.storeBackNet.Repositories;
using bookstore.storeBackNet.Models.Request;
using bookstore.storeBackNet.Models.Response;
using System.Data;
using Dapper;

namespace Libropedia.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<DapperContext> _contextMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _contextMock = new Mock<DapperContext>();
            _bookService = new BookService(_contextMock.Object);
        }

        [Fact]
        public async Task FilterBookAsync_ShouldReturnPaginatedResults()
        {
            // Arrange
            var bookRequest = new BookRequest
            {
                Titulo = "Test Book",
                IdAutor = 1,
                IdCategoria = 1,
                SortBy = "titulo",
                Direction = "ASC",
                Limite = 10,
                Offset = 0
            };

            var expectedBooks = new List<BookResponse>
            {
                new BookResponse
                {
                    IdLibro = 1,
                    Titulo = "Test Book 1",
                    Isbn = "1234567890",
                    AnioPublicacion = 2024,
                    Autor = "Test Author",
                    Categoria = "Test Category",
                    Rating = "4.5",
                    TotalRating = "10"
                }
            };

            var connection = new Mock<IDbConnection>();
            var multiResult = new Mock<GridReader>();

            _contextMock.Setup(x => x.CreateConnection())
                .Returns(connection.Object);

            connection.Setup(x => x.QueryMultiple(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                CommandType.StoredProcedure))
                .Returns(multiResult.Object);

            multiResult.Setup(x => x.ReadAsync<BookResponse>())
                .ReturnsAsync(expectedBooks);
            
            multiResult.Setup(x => x.ReadAsync<int>())
                .ReturnsAsync(new List<int> { 1 });

            // Act
            var result = await _bookService.FilterBookAsync(bookRequest);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(1);
            result.TotalRecords.Should().Be(1);
            result.Data.First().Titulo.Should().Be("Test Book 1");
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

            var connection = new Mock<IDbConnection>();
            _contextMock.Setup(x => x.CreateConnection())
                .Returns(connection.Object);

            connection.Setup(x => x.QueryAsync<BookResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
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

        [Fact]
        public async Task GetBookDetail_ShouldReturnDetailedBookInformation()
        {
            // Arrange
            long bookId = 1;
            var bookDetail = new
            {
                id_libro = 1L,
                titulo = "Test Book",
                isbn = "1234567890",
                anio_publicacion = 2024,
                editorial = "Test Editorial",
                resumen = "Test Summary",
                portada_url = "test.jpg",
                autor_id = 1L,
                autor_nombre = "John",
                autor_apellido = "Doe",
                autor_nombre_completo = "John Doe",
                autor_biografia = "Test Bio",
                autor_nacionalidad = "Test Nation",
                categoria_id = 1L,
                categoria_nombre = "Test Category",
                categoria_descripcion = "Test Description",
                calificacion_promedio = 4.5M,
                total_resenas = 10L,
                cinco_estrellas = 5L,
                cuatro_estrellas = 3L,
                tres_estrellas = 1L,
                dos_estrellas = 1L,
                una_estrella = 0L
            };

            var connection = new Mock<IDbConnection>();
            var multiResult = new Mock<GridReader>();

            _contextMock.Setup(x => x.CreateConnection())
                .Returns(connection.Object);

            connection.Setup(x => x.QueryMultipleAsync(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                CommandType.StoredProcedure))
                .ReturnsAsync(multiResult.Object);

            multiResult.Setup(x => x.ReadSingleAsync<dynamic>())
                .ReturnsAsync(bookDetail);
            
            multiResult.Setup(x => x.ReadAsync<dynamic>())
                .ReturnsAsync(new List<dynamic>());

            // Act
            var result = await _bookService.GetBookDetail(bookId);

            // Assert
            result.Should().NotBeNull();
            result.IdLibro.Should().Be(bookId);
            result.Titulo.Should().Be("Test Book");
            result.Isbn.Should().Be("1234567890");
            result.Autor.Should().NotBeNull();
            result.Categoria.Should().NotBeNull();
            result.Estadisticas.Should().NotBeNull();
        }
    }
}
