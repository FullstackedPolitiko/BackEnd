FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /App

COPY src/Backend.csproj ./
RUN dotnet restore

COPY src/ ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "Backend.dll"]