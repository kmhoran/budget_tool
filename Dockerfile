# Create the build environment image
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build-env
WORKDIR /app/
# copy files to current directory
COPY . ./

# publish to /out
RUN dotnet publish AppCli/AppCli.csproj -c Release -o /app/out
# change to published directory and set entrypoint
FROM mcr.microsoft.com/dotnet/core/runtime:2.2
WORKDIR /app/
COPY --from=build-env /app/out ./
RUN ls -a
ENTRYPOINT ["dotnet", "AppCli.dll"]
