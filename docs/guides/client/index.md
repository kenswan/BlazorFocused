---
uid: guides/client/index
title: BlazorFocused Client
---

# Client Overview

## App Settings

You can add Rest Client configurations in `wwwroot/appsettings.json` by adding a `RestClient` section

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Information"
    }
  },
  "RestClient": {
    "BaseAddress": "https://localhost:7074"
  },
  "AllowedHosts": "*"
}
```

## Startup

Program.cs (leverages appsettings.json)

```csharp
builder.Services.AddRestClient();
```

Program.cs (inline configuration)

```csharp
builder.Services.AddRestClient((httpClient) => {
    httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});
```
