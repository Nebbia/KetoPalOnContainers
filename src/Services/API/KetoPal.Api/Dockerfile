FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["src/KetoPal.Api/KetoPal.Api.csproj", "src/KetoPal.Api/"]
COPY ["src/KetoPal.Core/KetoPal.Core.csproj", "src/KetoPal.Core/"]
COPY ["src/KetoPal.Infrastructure/KetoPal.Infrastructure.csproj", "src/KetoPal.Infrastructure/"]

RUN dotnet restore "src/KetoPal.Api/KetoPal.Api.csproj"
COPY . .
WORKDIR "/src/src/KetoPal.Api"
RUN dotnet build "KetoPal.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KetoPal.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KetoPal.Api.dll"]