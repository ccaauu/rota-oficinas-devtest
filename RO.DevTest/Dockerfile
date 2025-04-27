FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.sln .
COPY RO.DevTest.*/*.csproj ./
RUN for f in *.csproj; do mkdir -p ${f%.*} && mv "$f" ${f%.*}/; done
COPY . .
WORKDIR /src/RO.DevTest.WebApi
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RO.DevTest.WebApi.dll"]
