#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MISA.CukCuk.Web/MISA.CukCuk.Web.csproj", "MISA.CukCuk.Web/"]
COPY ["MISA.ApplicationCore/MISA.ApplicationCore.csproj", "MISA.ApplicationCore/"]
COPY ["MISA.Infarstructure/MISA.Infarstructure.csproj", "MISA.Infarstructure/"]
RUN dotnet restore "MISA.CukCuk.Web/MISA.CukCuk.Web.csproj"
COPY . .
WORKDIR "/src/MISA.CukCuk.Web"
RUN dotnet build "MISA.CukCuk.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MISA.CukCuk.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MISA.CukCuk.Web.dll"]