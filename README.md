# Mediator pattern example with MediatR

> some code and steps from [medium.com](https://medium.com/dotnet-hub/use-mediatr-in-asp-net-or-asp-net-core-cqrs-and-mediator-in-dotnet-how-to-use-mediatr-cqrs-aspnetcore-5076e2f2880c) and [andrewlock.net](https://andrewlock.net/using-dependency-injection-in-a-net-core-console-application/)

## Make base project

```bash
# Make source folder
mkdir src && cd src
# Make solution
dotnet new sln -n MediatorDemo
# Make console project
dotnet new console -n Program
# Add dependencies
dotnet add Program/Program.csproj package MediatR --version 10.0.1
dotnet add Program/Program.csproj package MediatR.Extensions.Microsoft.DependencyInjection --version 10.0.1
dotnet add Program/Program.csproj package Microsoft.Extensions.DependencyInjection --version 6.0.0
dotnet add Program/Program.csproj package Microsoft.Extensions.Logging --version 6.0.0
dotnet add Program/Program.csproj package Microsoft.Extensions.Logging.Console --version 6.0.0
# Add projects to solution
dotnet sln add Program/Program.csproj
# Restore NuGet packages
dotnet restore
```