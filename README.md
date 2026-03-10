---

# DBGuard

**DBGuard** is a **schema-aware validation library for Entity Framework Core** that prevents common SQL Server errors before data reaches the database.

It automatically validates entity data against the **actual database schema** and throws clear, developer-friendly validation errors instead of cryptic SQL exceptions.

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
* EF Core SaveChanges interceptor
* Built-in logging support
* Validation diagnostics metrics
* Clear and developer-friendly error messages

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

# Quick Start

## 1. Configure DBGuard

Add DBGuard to your service collection.

```csharp
builder.Services.AddDbGuard(connectionString);
```

This loads the database schema and registers the validation engine.

---

## 2. Register EF Core DbContext

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

## 3. Example Entity

```csharp
public class User
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string Email { get; set; }
}
```

---

## 4. Example Database Table

```sql
CREATE TABLE Users
(
    Id INT IDENTITY PRIMARY KEY,
    FirstName VARCHAR(20),
    Email VARCHAR(50)
)
```

---

## 5. Example Insert

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

Contributions are welcome. Please submit pull requests or open issues.

---

# License

MIT License

---

# Author

Birupakhya Dash

---
