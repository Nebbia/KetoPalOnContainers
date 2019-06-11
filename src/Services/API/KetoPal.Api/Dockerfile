FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /build
COPY ["./src", "./src"]
COPY ["./test", "./test"]
COPY . .
RUN dotnet restore "KetoPal.sln"
WORKDIR "/build/src/Services/API/KetoPal.Api"
RUN dotnet build "KetoPal.Api.csproj" -c Release -o /app

FROM build as tests
WORKDIR /build
RUN dotnet test "KetoPal.sln"

FROM build AS publish
RUN dotnet publish "KetoPal.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KetoPal.Api.dll"]