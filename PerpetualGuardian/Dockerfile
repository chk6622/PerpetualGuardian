#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#==================== 1.compile =================================
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR .  
COPY ["PerpetualGuardian", "PerpetualGuardian"]
WORKDIR /PerpetualGuardian
RUN dotnet restore
RUN dotnet publish --framework netcoreapp3.1 --configuration Release --output /app/publish

#====================== 2.build image ==================================
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
LABEL Author = EthanXing
EXPOSE 8080/tcp
WORKDIR  /app
COPY --from=build /app/publish . 
ENTRYPOINT ["dotnet","PerpetualGuardian.dll"]