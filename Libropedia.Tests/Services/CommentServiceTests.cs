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
using bookstore.storeBackNet.DataContext;

namespace Libropedia.Tests.Services
{
    public class CommentServiceTests
    {
        private readonly Mock<IDapperContext> _contextMock;
        private readonly Mock<IDapperWrapper> _wrapperMock;
        private readonly CommentService _commentService;

        public CommentServiceTests()
        {
            _contextMock = new Mock<IDapperContext>();
             _wrapperMock = new Mock<IDapperWrapper>();
            _commentService = new CommentService(_contextMock.Object, _wrapperMock.Object);
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

             var connection = new Mock<IDbConnection>().Object;

           _contextMock.Setup(c => c.CreateConnection()).Returns(connection);

            _wrapperMock.Setup(d => d.ExecuteAsync(
            connection,
            It.IsAny<string>(),
            It.IsAny<object>(),
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
            var result = await _commentService.UpdateComments(updateRequest);

            // Assert
            result.Should().NotBeNull();
            result.IsError.Should().BeFalse();
            result.Message.Should().Be("Comentario modificado");
        }

      [Fact]
public async Task DeleteComment_ShouldReturnSuccess_WhenCommentIsDeleted()
{
    // Arrange
    var commentId = 123;
   var connection = new Mock<IDbConnection>().Object;
          _contextMock.Setup(x => x.CreateConnection())
        .Returns(connection);

    var wrapperMock = new Mock<IDapperWrapper>();
    _wrapperMock.Setup(w => w.ExecuteAsync(
        connection,
        It.IsAny<string>(),
        It.IsAny<object>(),
        null
    )).ReturnsAsync(1); // simulate 1 row affected

   

    // Act
    var result = await _commentService.DeleteComment(commentId);

    // Assert
    result.Should().NotBeNull();
    result.IsError.Should().BeFalse();
    result.Message.Should().Be("Reseña eliminada");
}
    }
}
