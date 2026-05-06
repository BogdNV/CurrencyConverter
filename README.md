# 💱 CurrencyConverter API

> ASP.NET Core Web API для конвертации валют с кэшированием и интеграцией с [Frankfurter API](https://www.frankfurter.dev/)

---

## 📋 О проекте

**CurrencyConverter** — это REST API сервис для получения актуальных курсов валют и выполнения конвертации. Проект демонстрирует лучшие практики разработки на ASP.NET Core: работа с внешними API, кэширование, валидация данных и чистая архитектура.

### ✨ Особенности
- 🔁 Конвертация между 30+ валютами (EUR, USD, GBP, RUB и др.)
- 🗄️ Кэширование ответов через `IMemoryCache` для снижения нагрузки на внешний API
- 📦 Типобезопасные модели данных через `record`
- 🧩 Dependency Injection для сервисов и `HttpClient`
- 📚 Автоматическая OpenAPI/Swagger-документация
- 🌐 Поддержка `CancellationToken` для корректной обработки отмены запросов

---

## 🛠 Технологический стек

| Компонент | Версия / Описание |
|-----------|------------------|
| **.NET** | .NET 8 / C# 12 |
| **Фреймворк** | ASP.NET Core Web API |
| **HTTP-клиент** | `IHttpClientFactory` + `System.Net.Http.Json` |
| **Кэширование** | `IMemoryCache` со скользящим истечением (1 час) |
| **Документация** | OpenAPI 3.0 + `Microsoft.AspNetCore.OpenApi` |
| **Внешний API** | [Frankfurter.dev](https://www.frankfurter.dev/) (бесплатно, без ключа) |

---

## 🚀 Быстрый старт

### Предварительные требования
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Доступ к интернету (для запросов к Frankfurter API)

### Установка и запуск

```bash
# 1. Клонируйте репозиторий
git clone https://github.com/BogdNV/CurrencyConverter.git
cd CurrencyConverter

# 2. Восстановите зависимости
dotnet restore

# 3. Запустите проект
dotnet run
