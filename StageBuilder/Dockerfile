# Get base SDK image from Microsoft
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /StageBuilder

# Copy only the csproj file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy project files and build release
COPY . ./
RUN dotnet publish -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /StageBuilder
EXPOSE 80/tcp
COPY --from=build-env /StageBuilder/out .
ENTRYPOINT ["dotnet", "StageBuilder.dll"]
