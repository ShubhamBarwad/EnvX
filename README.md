# EnvX

**EnvX** is a lightweight and efficient .NET library for managing environment variables across multiple projects in a solution. It allows you to load variables from a `.env` file located at the root of your solution, centralizing configuration management and making it easy to access environment-specific settings.

---

## Features

- **Centralized Configuration:** Manage environment variables in a single `.env` file for the entire solution.
- **Simple API:** Easily access variables using a static `ConfigManager` class.
- **Automatic Loading:** Automatically loads environment variables during application initialization.
- **Customizable:** Dynamically handle additional variables without modifying library code.

---

## Installation

Install the library via NuGet:

```bash
dotnet add package EnvX
