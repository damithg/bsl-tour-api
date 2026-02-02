# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY BSLTours.sln ./
COPY BSLTours.API/*.csproj ./BSLTours.API/
COPY Communications/BSLTours.Communications.Abstractions/*.csproj ./Communications/BSLTours.Communications.Abstractions/
COPY Communications/BSLTours.Communications.Core/*.csproj ./Communications/BSLTours.Communications.Core/
COPY Communications/BSLTours.Communications.SendGrid/*.csproj ./Communications/BSLTours.Communications.SendGrid/
COPY Communications/BSLTours.Communications.Postmark/*.csproj ./Communications/BSLTours.Communications.Postmark/

RUN dotnet restore BSLTours.API/BSLTours.API.csproj

# Copy everything and build
COPY . ./
RUN dotnet publish BSLTours.API/BSLTours.API.csproj -c Release -o /app/out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "BSLTours.API.dll"]
