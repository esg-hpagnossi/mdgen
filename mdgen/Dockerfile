FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["mdgen/mdgen.csproj", "mdgen/"]
RUN dotnet restore "mdgen/mdgen.csproj"
COPY . .
WORKDIR "/src/mdgen"
RUN dotnet build "mdgen.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mdgen.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mdgen.dll"]
