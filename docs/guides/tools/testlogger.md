---
uid: guides/tools/testlogger
---

# Test Logger

## Usage

```csharp
private readonly ITestLogger<TestService> testLogger;
private readonly ITestService testService;

public TestServiceTests()
{
    testLogger = ToolsBuilder.CreateTestLogger<TestService>();

    testService =
        new TestService(testLogger);
}

[Fact]
public async Task ShouldLogError()
{
    testService.GeneralMethodWithError();

    testLogger.VerifyWasCalledWith(LogLevel.Error)
}
```
