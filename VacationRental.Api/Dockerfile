#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 8081:80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["VacationRental.Api/VacationRental.Api.csproj", "VacationRental.Api/"]
RUN dotnet restore "VacationRental.Api/VacationRental.Api.csproj"
COPY . .
WORKDIR "/src/VacationRental.Api"
RUN dotnet build "VacationRental.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VacationRental.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8081:80
ENTRYPOINT ["dotnet", "VacationRental.Api.dll"]