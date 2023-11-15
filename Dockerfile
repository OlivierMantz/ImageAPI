# Use the ASP.NET Core runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the build stage to compile the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ImageAPI/ImageAPI.csproj", "./ImageAPI/"]
RUN dotnet restore "ImageAPI/ImageAPI.csproj"
COPY . .
WORKDIR "/src/ImageAPI"
RUN dotnet build "ImageAPI.csproj" -c Release -o /app/build
RUN dotnet publish "ImageAPI.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ImageAPI.dll"]
