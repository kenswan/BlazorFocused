// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace BlazorFocused.Tools.Utility;

public class TestServiceWithLogger
{
    private readonly ILogger<TestServiceWithLogger> logger;

    public TestServiceWithLogger(ILogger<TestServiceWithLogger> logger)
    {
        this.logger = logger;
    }

    public void LogError()
    {
        logger.LogError("this is a test error");
    }

    public void LogErrorWithMessage(string message)
    {
        logger.LogError(message);
    }

    public void LogErrorWithException(string exceptionMessage, string clientMessage)
    {
        logger.LogError(new TestServiceLoggerException(exceptionMessage), clientMessage);
    }

    public void LogInfo()
    {
        logger.LogInformation("this is test info");
    }

    public void LogInfoWithMessage(string message)
    {
        logger.LogInformation(message);
    }

    public void LogInfoWithException(string exceptionMessage, string clientMessage)
    {
        logger.LogInformation(new TestServiceLoggerException(exceptionMessage), clientMessage);
    }
}
