# C# Hangman AI Challenge

This project implements a Hangman game using the following technologies:

- C#
- .NET 8
- [ASP.NET Core](https://github.com/dotnet/aspnetcore)
- [XUnit](https://xunit.net/)

## How to run the Application

Skip the installer sections if you already have the following installed:
- .NET 8

### Manual Installation on macOS & Windows

1. **Install .NET 8**
- Visit the [dotnet website](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
- Download the SDK installer relevant to your computer.
- Follow the installation wizard with the default settings.

2. **Verify Installation**
   - Open a terminal/command line.
   - Type `dotnet --version`.
   - You should see `8.0.x` or similar.

### Package Installation

You can also install .NET 8 using a package manager:
- Chocolatey: Windows
- Homebrew: macOS
- [Linux Installation Guide](https://learn.microsoft.com/en-us/dotnet/core/install/linux?WT.mc_id=dotnet-35129-website)

## Running the Application

To run the service using the .NET runtime, follow these steps:

- Navigate to the root directory of your c# application in the terminal/command line. For this repository the command would be:
  - `cd csharp`

1. Run the following command to install the services dependencies: `dotnet restore`
2. Run the following command to start up the application: `dotnet run --project src/api/api.csproj`
  - It is also possible to `cd` into the `src/api` folder and instead run `dotnet run`.
  - `dotnet watch --project src/api/api.csproj` is often a good option when hot reloading is needed.
  - The app should be available at: `http://localhost:4567`
3. Execute the unit tests: `dotnet test`