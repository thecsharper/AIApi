#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY AIApi.sln ./
COPY AIApi/*.csproj ./AIApi/
COPY AIApi.CommandHandlers/*.csproj ./AIApi.CommandHandlers/
COPY AIApi.Commands/*.csproj ./AIApi.Commands/
COPY AIApi.Events/*.csproj ./AIApi.Events/
COPY AIApi.Commands/*.csproj ./AIApi.Commands/
COPY AIApi.Messages/*.csproj ./AIApi.Messages/
COPY AIApi.Services/*.csproj ./AIApi.Services/
COPY AIApi.AcceptanceTests/*.csproj ./AIApi.AcceptanceTests/
COPY AIApi.UnitTests/*.csproj ./AIApi.UnitTests/

RUN dotnet restore
COPY . .
WORKDIR "/src/AIApi"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AIApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Hack to fix tensorflow library name issue
RUN cp /app/bin/Debug/net7.0/runtimes/linux/native/libtensorflow.so /app/bin/Debug/net7.0/runtimes/linux/native/libtensorflow.so.1
RUN cp /app/bin/Debug/net7.0/runtimes/linux/native/libtensorflow_framework.so /app/bin/Debug/net7.0/runtimes/linux/native/libtensorflow_framework.so.1
ENTRYPOINT ["dotnet", "AIApi.dll"]