# Поздравлятор

Веб-приложение на ASP.NET Core MVC для ведения списка дней рождения.

## Технологии
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core + SQLite

## Как запустить
1. Установить .NET 8 SDK: https://dotnet.microsoft.com/download
2. Клонировать репозиторий:
   \`\`\`bash
   git clone https://github.com/ТВОЙ_ЛОГИН/pozdravlyator.git
   cd pozdravlyator
   \`\`\`
3. Восстановить зависимости и применить миграции:
   \`\`\`bash
   dotnet restore
   dotnet tool install --global dotnet-ef
   dotnet ef database update
   \`\`\`
4. Запустить:
   \`\`\`bash
   dotnet run
   \`\`\`
5. Открыть адрес из консоли в браузере.