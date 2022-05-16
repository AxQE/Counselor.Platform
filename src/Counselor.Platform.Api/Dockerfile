#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Counselor.Platform.Api/Counselor.Platform.Api.csproj", "src/Counselor.Platform.Api/"]
COPY ["src/Counselor.Platform.Data/Counselor.Platform.Data.csproj", "src/Counselor.Platform.Data/"]
RUN dotnet restore "src/Counselor.Platform.Api/Counselor.Platform.Api.csproj"
COPY . .
WORKDIR "/src/src/Counselor.Platform.Api"
RUN dotnet build "Counselor.Platform.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Counselor.Platform.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Counselor.Platform.Api.dll"]