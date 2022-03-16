FROM mcr.microsoft.com/dotnet/aspnet:5.0.15-alpine3.15 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0.406-alpine3.15 AS build
WORKDIR /src
COPY ["NotesUsers.csproj", "."]
RUN dotnet restore "NotesUsers.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "NotesUsers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotesUsers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotesUsers.dll"]
