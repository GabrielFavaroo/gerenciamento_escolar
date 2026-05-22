FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src
COPY ["Gerenciamento_Escolar/Gerenciamento_Escolar.csproj", "Gerenciamento_Escolar/"]
RUN dotnet restore "Gerenciamento_Escolar/Gerenciamento_Escolar.csproj"
COPY . .


WORKDIR "/src/Gerenciamento_Escolar"
RUN dotnet publish "./Gerenciamento_Escolar.csproj" -c Release -o /app/publish --no-self-contained /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Gerenciamento_Escolar.dll"]
