#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Counselor.Platform.Worker/Counselor.Platform.Worker.csproj", "src/Counselor.Platform.Worker/"]
COPY ["src/Counselor.Platform/Counselor.Platform.csproj", "src/Counselor.Platform/"]
COPY ["src/Counselor.Platform.Interpreter/Counselor.Platform.Interpreter.csproj", "src/Counselor.Platform.Interpreter/"]
COPY ["src/Counselor.Platform.Utils/Counselor.Platform.Utils.csproj", "src/Counselor.Platform.Utils/"]
COPY ["src/Counselor.Platform.Data/Counselor.Platform.Data.csproj", "src/Counselor.Platform.Data/"]
RUN dotnet restore "src/Counselor.Platform.Worker/Counselor.Platform.Worker.csproj"
COPY . .
WORKDIR "/src/src/Counselor.Platform.Worker"
RUN dotnet build "Counselor.Platform.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Counselor.Platform.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Counselor.Platform.Worker.dll"]