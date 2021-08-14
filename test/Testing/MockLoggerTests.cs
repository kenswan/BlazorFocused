using BlazorFocused.Test.Utility;
using Microsoft.Extensions.Logging;
using System;
using Xunit;
using Xunit.Abstractions;

namespace BlazorFocused.Testing.Test
{
    public class MockLoggerTests
    {
        private readonly MockLogger<TestServiceWithLogger> mockLogger;
        private readonly TestServiceWithLogger testServiceWithLogger;
        private readonly ITestOutputHelper testOutputHelper;

        public MockLoggerTests(ITestOutputHelper testOutputHelper)
        {
            mockLogger = new();
            testServiceWithLogger = new(mockLogger);
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ShouldPassIfLogCalled()
        {
            testServiceWithLogger.LogError();

            mockLogger.VerifyWasCalled();
        }

        [Fact]
        public void ShouldFailIfLogNotCalled()
        {
            Assert.Throws<MockLoggerException>(() => mockLogger.VerifyWasCalled());
        }

        [Fact]
        public void ShouldPassIfLogCalledWithLevel()
        {
            testServiceWithLogger.LogError();

            mockLogger.VerifyWasCalledWith(LogLevel.Error);
        }

        [Fact]
        public void ShouldFailIfLogNotCalledWithLevel()
        {
            testServiceWithLogger.LogInfo();

            Assert.Throws<MockLoggerException>(() => mockLogger.VerifyWasCalledWith(LogLevel.Error));
        }

        [Fact]
        public void ShouldPassIfLogCalledWithMessage()
        {
            var message = "This is a test message for an error";

            testServiceWithLogger.LogErrorWithMessage(message);

            mockLogger.VerifyWasCalledWith(LogLevel.Error, message);
        }

        [Fact]
        public void ShouldFailIfLogNotCalledWithMessage()
        {
            var expectedMessage = "This is the expected message for the error";
            var actualMessage = "This is the actual message for the error";

            testServiceWithLogger.LogErrorWithMessage(actualMessage);

            Assert.Throws<MockLoggerException>(() => 
                mockLogger.VerifyWasCalledWith(LogLevel.Error, expectedMessage));
        }

        [Fact]
        public void ShouldPassIfLogCalledWithException()
        {
            var exceptionMessage = "This is a test exception message for an error";
            var clientMessage = "This is a test client message for an error";
            var exception = new TestServiceLoggerException(exceptionMessage);

            testServiceWithLogger.LogErrorWithException(exceptionMessage, clientMessage);

            mockLogger.VerifyWasCalledWith(LogLevel.Error, exception, clientMessage);
        }

        [Fact]
        public void ShouldFailIfLogNotCalledWithException()
        {
            var exceptionMessage = "This is a test exception message for an error";
            var clientMessage = "This is a test client message for an error";
            var exception = new InvalidOperationException(exceptionMessage);

            testServiceWithLogger.LogErrorWithException(exceptionMessage, clientMessage);

            Assert.Throws<MockLoggerException>(() => 
                mockLogger.VerifyWasCalledWith(LogLevel.Error, exception, clientMessage));
        }

        [Fact]
        public void ShouldPassIfLogCalledWithExceptionType()
        {
            var exceptionMessage = "This is a test exception message for an error";
            var clientMessage = "This is a test client message for an error";

            testServiceWithLogger.LogErrorWithException(exceptionMessage, clientMessage);

            mockLogger.VerifyWasCalledWithException<TestServiceLoggerException>(LogLevel.Error);
        }

        [Fact]
        public void ShouldFailIfLogNotCalledWithExceptionType()
        {
            var exceptionMessage = "This is a test exception message for an error";
            var clientMessage = "This is a test client message for an error";

            testServiceWithLogger.LogErrorWithException(exceptionMessage, clientMessage);

            Assert.Throws<MockLoggerException>(() =>
                mockLogger.VerifyWasCalledWithException<InvalidOperationException>(LogLevel.Error));
        }

        [Fact]
        public void ShouldTrackOutput()
        {
            var exceptionMessage = "This is a test exception message for an error";
            var clientMessage = "This is a test client message for an error";
            var outputCount = 0;
            var outputMockLogger = new MockLogger<TestServiceWithLogger>((logLevel, message, exception) => 
            {
                testOutputHelper.WriteLine($"{logLevel} : {message} : {exception}");
                outputCount += 1;
            });
            var testServiceWithOutputLogger = new TestServiceWithLogger(outputMockLogger);

            testServiceWithOutputLogger.LogError();
            testServiceWithOutputLogger.LogErrorWithMessage(clientMessage);
            testServiceWithOutputLogger.LogErrorWithException(exceptionMessage, clientMessage);

            Assert.Equal(3, outputCount);
        }
    }
}
