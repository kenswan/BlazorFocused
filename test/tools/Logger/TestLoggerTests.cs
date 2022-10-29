using BlazorFocused.Tools.Extensions;
using BlazorFocused.Tools.Utility;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace BlazorFocused.Tools.Logger;

public class TestLoggerTests
{
    private readonly ITestLogger<TestServiceWithLogger> testLogger;
    private readonly TestServiceWithLogger testServiceWithLogger;
    private readonly ITestOutputHelper testOutputHelper;

    public TestLoggerTests(ITestOutputHelper testOutputHelper)
    {
        void logAction(LogLevel level, string message, Exception exception) =>
                    testOutputHelper.WriteTestLoggerMessage(level, message, exception);

        testLogger = new TestLogger<TestServiceWithLogger>(logAction);

        testServiceWithLogger = new(testLogger);
        this.testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void ShouldPassIfLogCalled()
    {
        testServiceWithLogger.LogError();

        testLogger.VerifyWasCalled();
    }

    [Fact]
    public void ShouldFailIfLogNotCalled()
    {
        void testCode() => testLogger.VerifyWasCalled();

        _ = Assert.Throws<TestLoggerException>(testCode);
    }

    [Fact]
    public void ShouldPassIfLogCalledWithLevel()
    {
        testServiceWithLogger.LogError();

        testLogger.VerifyWasCalledWith(LogLevel.Error);
    }

    [Fact]
    public void ShouldFailIfLogNotCalledWithLevel()
    {
        testServiceWithLogger.LogInfo();

        Assert.Throws<TestLoggerException>(() => testLogger.VerifyWasCalledWith(LogLevel.Error));
    }

    [Fact]
    public void ShouldPassIfLogCalledWithMessage()
    {
        var message = "This is a test message for an error";

        testServiceWithLogger.LogErrorWithMessage(message);

        testLogger.VerifyWasCalledWith(LogLevel.Error, message);
    }

    [Fact]
    public void ShouldFailIfLogNotCalledWithMessage()
    {
        var expectedMessage = "This is the expected message for the error";
        var actualMessage = "This is the actual message for the error";

        testServiceWithLogger.LogErrorWithMessage(actualMessage);

        Assert.Throws<TestLoggerException>(() =>
            testLogger.VerifyWasCalledWith(LogLevel.Error, expectedMessage));
    }

    [Fact]
    public void ShouldPassIfLogCalledWithException()
    {
        var exceptionMessage = "This is a test exception message for an error";
        var clientMessage = "This is a test client message for an error";
        var exception = new TestServiceLoggerException(exceptionMessage);

        testServiceWithLogger.LogErrorWithException(exceptionMessage, clientMessage);

        testLogger.VerifyWasCalledWith(LogLevel.Error, exception, clientMessage);
    }

    [Fact]
    public void ShouldFailIfLogNotCalledWithException()
    {
        var exceptionMessage = "This is a test exception message for an error";
        var clientMessage = "This is a test client message for an error";
        var exception = new InvalidOperationException(exceptionMessage);

        testServiceWithLogger.LogErrorWithException(exceptionMessage, clientMessage);

        Assert.Throws<TestLoggerException>(() =>
            testLogger.VerifyWasCalledWith(LogLevel.Error, exception, clientMessage));
    }

    [Fact]
    public void ShouldPassIfLogCalledWithExceptionType()
    {
        var exceptionMessage = "This is a test exception message for an error";
        var clientMessage = "This is a test client message for an error";

        testServiceWithLogger.LogErrorWithException(exceptionMessage, clientMessage);

        testLogger.VerifyWasCalledWithException<TestServiceLoggerException>(LogLevel.Error);
    }

    [Fact]
    public void ShouldFailIfLogNotCalledWithExceptionType()
    {
        var exceptionMessage = "This is a test exception message for an error";
        var clientMessage = "This is a test client message for an error";

        testServiceWithLogger.LogErrorWithException(exceptionMessage, clientMessage);

        Assert.Throws<TestLoggerException>(() =>
            testLogger.VerifyWasCalledWithException<InvalidOperationException>(LogLevel.Error));
    }

    [Fact]
    public void ShouldTrackOutput()
    {
        var exceptionMessage = "This is a test exception message for an error";
        var clientMessage = "This is a test client message for an error";
        var outputCount = 0;
        var outputMockLogger = new TestLogger<TestServiceWithLogger>((logLevel, message, exception) =>
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
