# Используем SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Копируем ВСЕ файлы проекта сразу
COPY . .

# Восстанавливаем пакеты и публикуем одной командой
# Убираем --no-restore, чтобы dotnet сам разобрался с зависимостями
RUN dotnet publish "src/GymDesk.API/GymDesk.API.csproj" -c Release -o /app/publish --no-restore || \
    dotnet publish "src/GymDesk.API/GymDesk.API.csproj" -c Release -o /app/publish

# Финальный образ только с runtime (он легкий)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GymDesk.API.dll"]