#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

COPY *.csproj ./
RUN dotnet restore
COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/sdk:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AuthService.dll"]

#FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
#WORKDIR /src
#COPY ["AuthService.csproj", "."]
#RUN dotnet restore "./AuthService.csproj"
#COPY . .
#WORKDIR "/src/."
#RUN dotnet build "AuthService.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "AuthService.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "AuthService.dll"]