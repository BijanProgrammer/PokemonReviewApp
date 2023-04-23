# Pokemon Review App

## Setup

Create a file named `appsettings.Development.json`
in the `PokemonReviewApp` folder
and fill it like this:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```
