# Cirreum.Exceptions

[![NuGet Version](https://img.shields.io/nuget/v/Cirreum.Exceptions.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.Exceptions/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Cirreum.Exceptions.svg?style=flat-square&labelColor=1F1F1F&color=003D8F)](https://www.nuget.org/packages/Cirreum.Exceptions/)
[![License](https://img.shields.io/github/license/cirreum/Cirreum.Exceptions?style=flat-square&labelColor=1F1F1F&color=F2F2F2)](https://github.com/cirreum/Cirreum.Exceptions/blob/main/LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-003D8F?style=flat-square&labelColor=1F1F1F)](https://dotnet.microsoft.com/)

**Application exception types for the Cirreum Framework**

## Overview

**Cirreum.Exceptions** provides a standardized set of application-level exception types that map directly to HTTP status codes. These exceptions serve as the foundation for consistent error handling across all Cirreum libraries and applications.

This library is part of the **Base layer** of the Cirreum ecosystem, meaning it has zero dependencies on other Cirreum packages and is fully AOT-compatible.

## Features

- **Zero Dependencies** - No external package references
- **AOT Compatible** - Full Native AOT support with `IsAotCompatible=true`
- **HTTP-Aligned** - Exception types map directly to HTTP status codes
- **RFC 7807 Support** - `ExceptionModel` for Problem Details serialization
- **Validation Models** - `FailureModel` for structured validation errors

## Exception Types

| Exception | HTTP Status | Use Case |
|-----------|:-----------:|----------|
| `BadRequestException` | 400 | Malformed or invalid request data |
| `UnauthorizedException` | 401 | Missing or invalid authentication |
| `ForbiddenException` | 403 | Authenticated but not authorized |
| `NotFoundException` | 404 | Resource not found |
| `ConflictException` | 409 | Resource state conflict |
| `ConcurrencyException` | 409 | Optimistic concurrency violation |
| `ServerException` | 500 | Internal server error |

## Installation

```bash
dotnet add package Cirreum.Exceptions
```

## Usage

### Throwing Exceptions

```csharp
using Cirreum.Exceptions;

// Resource by Id not found
throw new NotFoundException(userId);

// Authorization failure
throw new ForbiddenException("You do not have access to this resource");

// Validation failure
throw new BadRequestException("Email address is invalid");

// Concurrency conflict
throw new ConcurrencyException(entityId);
```

### ExceptionModel for Problem Details

The `ExceptionModel` class provides RFC 7807 Problem Details support:

```csharp
using Cirreum.Exceptions;

var model = new ExceptionModel {
    Type = "https://example.com/errors/not-found",
    Title = "Resource Not Found",
    Status = 404,
    Detail = "The requested user was not found",
    Instance = "/api/users/123"
};

// Serialize to application/problem+json
var json = JsonSerializer.Serialize(model);
```

Extension data is supported via `JsonExtensionData`:

```csharp
var model = new ExceptionModel {
    Title = "Validation Failed",
    Status = 400,
    Extensions = new Dictionary<string, JsonElement> {
        ["errors"] = JsonSerializer.SerializeToElement(validationErrors)
    }
};
```

### FailureModel for Validation Errors

```csharp
using Cirreum.Exceptions;

var failure = new FailureModel {
    PropertyName = "Email",
    ErrorMessage = "Email address is not valid",
    ErrorCode = "EmailValidator",
    AttemptedValue = "not-an-email"
};
```

### Fatal Exception Detection

The `IsFatal` extension method identifies exceptions that should not be caught:

```csharp
using System;

try {
    // ... operation
}
catch (Exception ex) when (!ex.IsFatal()) {
    // Safe to handle - not OOM, StackOverflow, or ThreadAbort
    logger.LogError(ex, "Operation failed");
}
```

## Integration

### ASP.NET Core

Cirreum.Exceptions integrates seamlessly with ASP.NET Core's exception handling:

```csharp
app.UseExceptionHandler(builder => {
    builder.Run(async context => {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var (statusCode, model) = exception switch {
            NotFoundException ex => (404, ex.ToExceptionModel()),
            ForbiddenException ex => (403, ex.ToExceptionModel()),
            BadRequestException ex => (400, ex.ToExceptionModel()),
            _ => (500, new ExceptionModel { Title = "Internal Server Error", Status = 500 })
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(model);
    });
});
```

### Cirreum Ecosystem

This library provides the exception foundation for:

- **Cirreum.Core** - Application core uses these exception types
- **Cirreum.Persistence.Sql** - Database operations throw these exceptions
- **Cirreum.Persistence.Azure** - Azure storage operations use these exceptions
- **Cirreum.Runtime** - Runtime exception handling and middleware

## Design Principles

### HTTP-First Design
Exception types are designed around HTTP semantics, making it trivial to translate application errors to appropriate HTTP responses.

### AOT Compatibility
All types are fully compatible with Native AOT compilation. The `ExceptionModel.Extensions` property uses `IDictionary<string, JsonElement>` instead of `object` to ensure AOT-safe serialization.

### Zero Dependencies
This package has no external dependencies, keeping the dependency graph clean for Base layer consumers.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Cirreum Foundation Framework**
*Layered simplicity for modern .NET*
