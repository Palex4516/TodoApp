## https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy everything else and build app
COPY . ./TodoApp
WORKDIR /source/TodoApp
RUN dotnet build
RUN dotnet publish *.csproj -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "ToDoList.dll"]

