# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY app/HabitTracker/HabitTracker.csproj ./app/HabitTracker/
COPY app/HabitTracker.Domain/HabitTracker.Domain.csproj ./app/HabitTracker.Domain/
COPY app/HabitTracker.Persistence/HabitTracker.Persistence.csproj ./app/HabitTracker.Persistence/
RUN dotnet restore ./app/HabitTracker

# copy everything else and build app
COPY app/. ./app/
RUN dotnet publish ./app/HabitTracker -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "HabitTracker.dll"]