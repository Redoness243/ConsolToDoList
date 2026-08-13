# C# & SQLite Task Tracker

A console-based task management application built with C# and SQLite (`Microsoft.Data.Sqlite`) for local database storage. This project demonstrates database integration, Repository Pattern principles, and LINQ queries for data manipulation.

## 🚀 Features

- **Task Management (CRUD):** Create, read, update, and delete tasks seamlessly via the console interface.
- **Persistent Storage:** Local data persistence powered by `SQLite` with automatic `sql.db` file handling.
- **Repository Pattern:** Clean separation of concerns with dedicated `Save` and `Load` repository methods.
- **Data Querying:** Fast data filtering and collection processing using LINQ.

## 🛠️ Tech Stack & Packages

- **Language:** C#
- **Framework / Runtime:** .NET
- **Database:** SQLite
- **NuGet Packages:** `Microsoft.Data.Sqlite`

## 🏁 How to Run

1. Open your terminal in the project root directory.
2. Run the following command:
   ```bash
   dotnet run