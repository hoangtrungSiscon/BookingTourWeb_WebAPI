# Base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BookingTourWeb_WebAPI/BookingTourWeb_WebAPI.csproj", "BookingTourWeb_WebAPI/"]
RUN dotnet restore "BookingTourWeb_WebAPI/BookingTourWeb_WebAPI.csproj"
COPY . .
WORKDIR "/src/BookingTourWeb_WebAPI"
RUN dotnet build "BookingTourWeb_WebAPI.csproj" -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish "BookingTourWeb_WebAPI.csproj" -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookingTourWeb_WebAPI.dll"]
