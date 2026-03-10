# DBGuard

DBGuard is a schema-aware validation library for EF Core.

It prevents common SQL errors like:

String or binary data would be truncated.

## Features

- Schema-aware validation
- Column length validation
- Decimal precision validation
- Null validation
- EF Core SaveChanges interception
- Logging and diagnostics

## Installation

Install via NuGet:

Install-Package DBGuard

## Usage

services.AddDbGuard(connectionString);

## Example Error

Table: Users  
Column: FirstName  
Allowed Length: 50  
Actual Length: 72
