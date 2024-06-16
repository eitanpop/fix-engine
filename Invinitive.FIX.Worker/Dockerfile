#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Invinitive.FIX.Worker/Invinitive.FIX.Worker.csproj", "Invinitive.FIX.Worker/"]
COPY ["Invinitive.FIX.Application/Invinitive.FIX.Application.csproj", "Invinitive.FIX.Application/"]
COPY ["Invinitive.Fix.Engine/Invinitive.FIX.Engine.csproj", "Invinitive.Fix.Engine/"]
RUN dotnet restore "Invinitive.FIX.Worker/Invinitive.FIX.Worker.csproj"
COPY . .
WORKDIR "/src/Invinitive.FIX.Worker"
RUN dotnet build "Invinitive.FIX.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Invinitive.FIX.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Invinitive.FIX.Worker.dll"]