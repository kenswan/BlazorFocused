---
uid: guides/tools/index
title: BlazorFocused Tools
---

# Tools Overview

Leverage `ToolsBuilder` class to access BlazorFocused tooling

```csharp
ISimulatedHttp simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);

using HttpClient httpClient = simulatedHttp.HttpClient;
```

```csharp
ILogger<TestClass> logger = ToolsBuilder.CreateTestLogger<TestClass>();
```
