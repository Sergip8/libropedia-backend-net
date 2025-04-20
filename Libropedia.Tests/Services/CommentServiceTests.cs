using Xunit;
using Moq;
using FluentAssertions;
using Api.FunctionApp.DataContext;
using bookstore.storeBackNet.Repositories;
using bookstore.storeBackNet.Models.Request;
using bookstore.storeBackNet.Models.Response;
using System.Data;
using Dapper;
using Consultorio.Function.Models;

namespace Libropedia.Tests.Services
{
    public class CommentServiceTests
    {
        private readonly Mock<DapperContext> _contextMock;
        private readonly CommentService _commentService;

        public CommentServiceTests()
        {
            _contextMock = new Mock<DapperContext>();
            _commentService = new CommentService(_contextMock.Object);
        }

        [Fact]
        public async Task StoreComments_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var commentRequest = new CommentRequest
            {
                IdLibro = 1,
                IdUsuario = 1,
                Calificacion = 5,
                Comentario = "Great book!"
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
            var result = await _commentService.StoreComments(commentRequest);

            // Assert
            result.Should().NotBeNull();
            result.IsError.Should().BeFalse();
            result.Message.Should().Be("Comentario agregado");
        }

        [Fact]
        public async Task UpdateComments_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var updateRequest = new CommentUpdateRequest
            {
                IdResena = 1,
                IdUsuario = 1,
                Calificacion = 4,
                Comentario = "Updated review"
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
            var result = await _commentService.UpdateComments(updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.IsError.Should().BeFalse();
            result.Message.Should().Be("Comentario modificado");
        }

        [Fact]
        public async Task GetUserComments_ShouldReturnPaginatedComments()
        {
            // Arrange
            var request = new CommentUserRequest
            {
                UserId = 1,
                Limit = 10,
                Offset = 0
            };

            var expectedComments = new List<CommentUserResponse>
            {
                new CommentUserResponse
                {
                    IdResena = 1,
                    Calificacion = 5,
                    Comentario = "Test comment",
                    FechaResena = DateTime.Now,
                    IdLibro = 1,
                    Titulo = "Test Book",
                    AnioPublicacion = 2024,
                    Editorial = "Test Editorial",
                    PortadaUrl = "test.jpg"
                }
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

            multiResult.Setup(x => x.ReadAsync<CommentUserResponse>())
                .ReturnsAsync(expectedComments);
            
            multiResult.Setup(x => x.ReadAsync<int>())
                .ReturnsAsync(new List<int> { 1 });

            // Act
            var result = await _commentService.getUserComments(request);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(1);
            result.TotalRecords.Should().Be(1);
            result.Data.First().Comentario.Should().Be("Test comment");
        }

        [Fact]
        public async Task DeleteComment_ShouldReturnSuccess()
        {
            // Arrange
            int commentId = 1;
            var connection = new Mock<IDbConnection>();
            _contextMock.Setup(x => x.CreateConnection())
                .Returns(connection.Object);

            connection.Setup(x => x.QueryAsync(
                It.IsAny<string>(),
                It.IsAny<object>()))
                .ReturnsAsync(new List<dynamic>());

            // Act
            var result = await _commentService.DeleteComment(commentId);

            // Assert
            result.Should().NotBeNull();
            result.IsError.Should().BeFalse();
            result.Message.Should().Be("Reseña eliminada");
        }
    }
}
