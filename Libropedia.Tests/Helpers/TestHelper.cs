using System.Data;
using Moq;
using Api.FunctionApp.DataContext;
using Dapper;

namespace Libropedia.Tests.Helpers
{
    public static class TestHelper
    {
        public static Mock<DapperContext> CreateMockDapperContext()
        {
            return new Mock<DapperContext>();
        }

        public static Mock<IDbConnection> CreateMockDbConnection()
        {
            return new Mock<IDbConnection>();
        }

        public static void SetupBasicDatabaseMock(Mock<DapperContext> contextMock, Mock<IDbConnection> connectionMock)
        {
            contextMock.Setup(x => x.CreateConnection())
                .Returns(connectionMock.Object);
        }

        public static void SetupQueryAsync<T>(Mock<IDbConnection> connectionMock, IEnumerable<T> expectedResult)
        {
            connectionMock.Setup(x => x.QueryAsync<T>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                It.IsAny<CommandType>()))
                .ReturnsAsync(expectedResult);
        }

        public static void SetupQueryFirstOrDefaultAsync<T>(Mock<IDbConnection> connectionMock, T expectedResult)
        {
            connectionMock.Setup(x => x.QueryFirstOrDefaultAsync<T>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                It.IsAny<CommandType>()))
                .ReturnsAsync(expectedResult);
        }

        public static void SetupExecuteAsync(Mock<IDbConnection> connectionMock, int expectedResult)
        {
            connectionMock.Setup(x => x.ExecuteAsync(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                It.IsAny<CommandType>()))
                .ReturnsAsync(expectedResult);
        }

      
    }
}
