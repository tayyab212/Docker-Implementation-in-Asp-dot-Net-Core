


FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["WebApplicationMySQL.csproj", "."]
RUN dotnet restore "./WebApplicationMySQL.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "WebApplicationMySQL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApplicationMySQL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApplicationMySQL.dll","--environment=Production"]