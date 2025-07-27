FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /app
COPY . .
RUN dotnet restore "./DataImporter.csproj"
RUN dotnet build "DataImporter.csproj" \
    -c Release \
    --no-restore \
    --runtime alpine-x64 \
    --self-contained true \
    -p:PublishSingleFile=true

FROM build AS publish
RUN dotnet publish "DataImporter.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    --no-build \
    --runtime alpine-x64 \
    --self-contained true \
    -p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["./import.sh"]