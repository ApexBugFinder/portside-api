
#FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
FROM mcr.microsoft.com/dotnet/aspnet:3.1-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS=http://+:80

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser
#FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build

FROM mcr.microsoft.com/dotnet/sdk:3.1-focal AS build
WORKDIR /src


COPY ["./Portfolio/Portfolio.WebApp/Portfolio.WebApp.csproj", "./Portfolio/Portfolio.WebApp/"]

COPY ["./Portfolio/Portfolio.PorfolioDomain.Core/Portfolio.PorfolioDomain.Core.csproj", "./Portfolio/Portfolio.PorfolioDomain.Core/"]


RUN dotnet restore "./Portfolio/Portfolio.PorfolioDomain.Core/Portfolio.PorfolioDomain.Core.csproj"
RUN dotnet restore "./Portfolio/Portfolio.WebApp/Portfolio.WebApp.csproj"

# WORKDIR "/src/Portfolio/Portfolio.PorfolioDomain.Core"
# COPY . .
# RUN dotnet build "Portfolio.PorfolioDomain.Core.csproj" -c Release -o /app/build


WORKDIR "/src/Portfolio/Portfolio.WebApp"
COPY . .
RUN dotnet build "Portfolio.WebApp.csproj" -c Release -o /app/build





FROM build AS publish
RUN dotnet publish "Portfolio.WebApp.csproj" -c Release -o /app/publish/

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portfolio.WebApp.dll"]
