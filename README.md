# Skystruct

A domain-specific language (DSL) for defining readable data structures and generating idiomatic C# source code from them.

Skystruct is designed to be understood by both developers and non-technical domain experts. Definitions are written in a natural-language-inspired syntax and compiled into clean, hand-readable C# classes.

---

## How it works

A Skystruct definition file describes your data structures using a minimal syntax:

```
Define Person {
    Text Name
    required Number Age
    optional Text Biography
}

Define Employee is Person {
    Text EmployeeID
    Text Title
    decimal Number Salary
    Person Boss
}
```

Running Skystruct against this file generates the following C#:

```csharp
public class Person
{
    public string Name { get; set; }
    public long Age { get; set; }
    public string? Biography { get; set; }
}

public class Employee : Person
{
    public string EmployeeID { get; set; }
    public string Title { get; set; }
    public decimal Salary { get; set; }
    public Person Boss { get; set; }
}
```

---

## Syntax overview

A type definition starts with `Define`, followed by a name and a block of properties:

```
Define TypeName {
    [constraint] [type] PropertyName
}
```

### Types

| Skystruct | Generated C# |
|-----------|-------------|
| `Text`    | `string`    |
| `Number`  | `long`      |
| `Bool`    | `bool`      |
| Any defined type | The corresponding class |

### Constraints

| Constraint | Effect |
|------------|--------|
| `optional` | Generates a nullable type (`string?`, `long?`) |
| `required` | Marks the field as required |
| `list`     | Generates a `List<T>` |
| `decimal`  | Refines `Number` to `decimal` |

### Inheritance

Use `is` to inherit from another defined type:

```
Define Employee is Person {
    Text EmployeeID
}
```

---

## Installation

Skystruct is not yet published to NuGet and must be built locally.

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)

### Build locally

Clone the repository and build the project:

```bash
git clone https://github.com/yourusername/skystruct
cd skystruct
dotnet build
```

To pack it as a local NuGet package:

```bash
dotnet pack --configuration Release
```

This produces a `.nupkg` file in the `bin/Release` folder. You can then reference it in another project using a local NuGet source:

```bash
dotnet nuget add source /path/to/skystruct/bin/Release --name skystruct-local
dotnet add package Skystruct
```

---

## Usage examples

### Single type

```
Define UserProfile {
    Text Username
    Text Email
    Number Age
    optional Text Biography
}
```

### Nested types

```
Define Address {
    Text Street
    Text City
    Text PostalCode
}

Define Customer {
    Text Name
    required Text Email
    Address ShippingAddress
}
```

### Lists and decimals

```
Define Order {
    required Text OrderId
    required decimal Number TotalAmount
    list Product Items
}

Define Product {
    Text Name
    decimal Number Price
}
```

---
