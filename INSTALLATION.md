# 📌 Інструкція з встановлення "Flag it up!"

## ⚙️ Необхідні компоненти
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (або VS Code)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (для бази даних)
  
## 🚀 Швидкий старт
1. Клонуйте репозиторій:
   
git clone https://github.com/ваш-логін/flag-it-up.git
cd flag-it-up

2. Відновіть залежності:

dotnet restore


3. Налаштуйте базу даних:
- Оновіть рядок підключення в `appsettings.json`
  
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FlagItUp;Trusted_Connection=True;"
}

4. Застосуйте міграції:

dotnet ef database update

5. Запустіть проект:
   
dotnet run


## 🛠️ Альтернативний запуск через Visual Studio
1. Відкрийте `FlagItUp.sln`
2. Натисніть `Ctrl+F5` для запуску без дебагу
3. Або `F5` для запуску з дебагом

## 🌐 Доступ до додатка
Після запуску додаток буде доступний за адресою:

https://localhost:5000

## 📦 Інтеграція з IDE
Для VS Code додайте такі розширення:
- C# (від Microsoft)
- SQL Server (mssql)
- ESLint (Для FrontEnd)

## 🧪 Тестування
Для запуску тестів виконайте:

dotnet test


## ⚠️ Поширені проблеми
1. **Помилки підключення до БД**:
   - Перевірте, чи запущено SQL Server
   - Впевніться, що рядок підключення вірний

2. **Відсутні залежності**:
   
   dotnet restore
   

3. **Проблеми з міграціями**:
   
   dotnet ef migrations remove
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   
## 📞 Підтримка
Виникли питання? Створіть issue або пишіть на flagitup_support@gmail.com

📅 Останнє оновлення: 14/05/2025
🏷️ Версія: 1.0.0
