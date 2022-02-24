---
uid: guides/tools/simulatedhttp
---

# Simulated Http Client

## Usage

```csharp
private readonly ISimulatedHttp simulatedHttp;
private readonly ITestClient testClient;

public TestClientTests()
{
    simulatedHttp = ToolsBuilder.CreateSimulatedHttp("https://blazorfocused.net");

    restClient =
        new TestClient(simulatedHttp.HttpClient);
}

[Fact]
public async Task ShouldPerformHttpRequest()
{
    var url = "api/test";
    var request = new TestClass { Name = "Testing" };
    HttpStatusCode statusCode = HttpStatusCode.OK;
    var response = new TestClass { Id = "123", Name = "Testing" };

    simulatedHttp.SetupGET(HttpMethod.Get, url, request)
        .ReturnsAsync(statusCode, response);

    var actualResponse = await testClient.GetAsync<SimpleClass>(url, request);

    actualResponse.Should().BeEquivalentTo(response);

    simulatedHttp.VerifyWasCalled(httpMethod, url);
}
```
