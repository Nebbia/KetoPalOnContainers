FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/KetoPal.Identity/KetoPal.Identity.csproj", "src/KetoPal.Identity/"]
RUN dotnet restore "src/KetoPal.Identity/KetoPal.Identity.csproj"
COPY . .
WORKDIR "/src/src/KetoPal.Identity"
RUN dotnet build "KetoPal.Identity.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KetoPal.Identity.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KetoPal.Identity.dll"]
