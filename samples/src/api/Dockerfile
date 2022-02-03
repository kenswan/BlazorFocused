#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Samples.Api/Samples.Api.csproj", "src/Samples.Api/"]
RUN dotnet restore "src/Samples.Api/Samples.Api.csproj"
COPY . .
WORKDIR "/src/src/Samples.Api"
RUN dotnet build "Samples.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Samples.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Samples.Api.dll"]