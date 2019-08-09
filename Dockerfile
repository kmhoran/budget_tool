# Create the build environment image
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build-env
WORKDIR /app
 
# Copy the project file and restore the dependencies
COPY *.csproj ./
RUN dotnet restore
 
# Copy the remaining source files and build the application
COPY . ./
RUN dotnet publish -c Release -o out
 
# Build the runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "AppCli.dll"]