# Create the build environment image
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build-env

# copy files to current directory
COPY . ./

# publish to /out
RUN dotnet publish AppCli/AppCli.csproj -c Release -o /out

# change to published directory and set entrypoint
WORKDIR /out
FROM mcr.microsoft.com/dotnet/core/runtime:2.2
ENTRYPOINT ["dotnet", "AppCli.dll"]
