## Keywords

EF Core validation  
SQL Server validation  
Prevent "String or binary data would be truncated" error  
Entity Framework Core interceptor  
Database schema validation  
---
![NuGet Version](https://img.shields.io/nuget/v/DBGuard)
![NuGet Downloads](https://img.shields.io/nuget/dt/DBGuard)
![License](https://img.shields.io/github/license/birupakhya2000/dbguard)
![GitHub Stars](https://img.shields.io/github/stars/birupakhya2000/dbguard)
# DBGuard – EF Core Schema Validation Library

**DBGuard** is a **schema-aware validation library for Entity Framework Core** that prevents common SQL Server errors before data reaches the database.

It automatically validates entity data against the **actual database schema** and throws **clear, developer-friendly validation errors** instead of cryptic SQL exceptions.

---

# Why DBGuard?

When saving data using EF Core, SQL Server often throws errors like:

```
String or binary data would be truncated
```

This error does **not tell you**:

* Which table failed
* Which column caused the issue
* What the allowed size is
* What the actual value size is

DBGuard solves this problem by validating data **before the database insert happens**.

Example DBGuard error:

```
DBGuard Validation Failed

Table: Users
Column: FirstName
Allowed Length: 20
Actual Length: 45

Suggestion: Trim input or increase column length
```

---

# Features

* Schema-aware validation using actual database schema
* Automatic validation before `SaveChanges`
* Column length validation
* Null constraint validation
* Data type validation
* Decimal precision validation
* Numeric overflow validation
* Date validation
* EF Core `SaveChanges` interceptor
* Built-in logging support
* Validation diagnostics metrics
* Clear developer-friendly error messages

---

# Requirements

* .NET 8+
* Entity Framework Core
* SQL Server

---

# Installation

Install via **NuGet Package Manager**

```
Install-Package DBGuard
```

or using **.NET CLI**

```
dotnet add package DBGuard
```

---

# Getting Started

Follow these steps to start using **DBGuard**.

---

# Step 1 — Configure DBGuard

Add DBGuard to your service collection.

```csharp
builder.Services.AddDbGuard(connectionString);
```

This loads the database schema and registers the validation engine.

---

# Step 2 — Register EF Core DbContext

```csharp
builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    var interceptor = sp.GetRequiredService<DbGuardInterceptor>();

    options.UseSqlServer(connectionString)
           .AddInterceptors(interceptor);
});
```

DBGuard will now automatically validate entities before saving.

---

# Step 3 — Example Entity

```csharp
public class User
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string Email { get; set; }
}
```

---

# Step 4 — Example Database Table

```sql
CREATE TABLE Users
(
    Id INT IDENTITY PRIMARY KEY,
    FirstName VARCHAR(20),
    Email VARCHAR(50)
)
```

---

# Step 5 — Example Insert

```csharp
var user = new User
{
    FirstName = "THIS_IS_A_VERY_LONG_NAME_EXCEEDING_COLUMN_LIMIT",
    Email = "test@test.com"
};

context.Users.Add(user);

await context.SaveChangesAsync();
```

---

# Example Error Output

Instead of SQL Server throwing:

```
String or binary data would be truncated
```

DBGuard throws:

```
DBGuard Validation Failed

Table: Users
Column: FirstName
Allowed Length: 20
Actual Length: 45

Suggestion: Trim input or increase column length
```

This makes debugging significantly easier.

---

# Logging

DBGuard integrates with **Microsoft.Extensions.Logging**.

Example configuration:

```csharp
builder.Services.AddLogging(config =>
{
    config.AddConsole();
});
```

Example log output:

```
warn: DBGuardInterceptor

DBGuard validation failed.
Table: Users
Column: FirstName
Error: FirstName exceeds max length 20
```

---

# Diagnostics Metrics

DBGuard tracks frequently failing columns to help diagnose issues.

Example usage:

```csharp
var metrics = serviceProvider.GetRequiredService<ValidationMetrics>();

var failures = metrics.GetTopFailures();

foreach (var f in failures)
{
    Console.WriteLine($"{f.Key} -> {f.Value}");
}
```

Example output:

```
Users.FirstName -> 3 failures
Users.Email -> 1 failure
```

This helps identify problematic fields in large systems.

---

# Supported Validations

DBGuard currently supports:

* String length validation
* Null validation
* Data type validation
* Decimal precision validation
* Numeric overflow validation
* Date validation

---

# Project Architecture

```
DBGuard
│
├── DBGuard.Core
├── DBGuard.Schema
├── DBGuard.Validation
├── DBGuard.SqlServer
├── DBGuard.EFCore
│
├── samples
│   └── DBGuard.Demo
│
└── tests
    └── DBGuard.Tests
```

---

# Example Use Case

Without DBGuard:

```
SqlException: String or binary data would be truncated
```

With DBGuard:

```
DBGuard Validation Failed

Table: Products
Column: Description
Allowed Length: 100
Actual Length: 140
```

Developers immediately know **which column caused the issue**.

---

# Troubleshooting

### Login failed for user

Ensure your connection string is correct and you are using either:

Windows Authentication:

```
Trusted_Connection=True
```

or SQL Authentication:

```
User Id=yourUser;
Password=yourPassword;
```

Do not use both together.

---

### Table not found in schema

Ensure the table exists in the database and DBGuard can access it.

---

# Roadmap

Future improvements:

* PostgreSQL support
* MySQL support
* SQL error analyzer
* Performance optimizations
* Monitoring dashboard
* Additional validation rules

---

# Contributing

Contributions are welcome.

1. Fork the repository
2. Create a feature branch
3. Submit a pull request

---

# License

MIT License

---

# Author

Birupakhya Dash

---

**serious open-source library**.
