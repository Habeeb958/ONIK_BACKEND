# Use the official .NET 8 SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the .csproj file and restore any dependencies (via dotnet restore)
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application files into the container
COPY . ./

# Build and publish the application to the "out" directory
RUN dotnet publish -c Release -o out

# Use the official .NET 8 Runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Copy the built application from the "build" stage
COPY --from=build /app/out .

# Define the entry point for the application
#ENTRYPOINT ["dotnet", "YourAppName.dll"]

ENTRYPOINT ["dotnet", "ONIK BANK.dll"]