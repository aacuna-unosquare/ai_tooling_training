# Javascript Hangman API 

This partial implementation uses the following technologies:

- Node version 22
- Node Package Manager (included with Node)
- [Express](https://expressjs.com/): A micro web framework used to expose API endpoints.
- [Jest](https://jestjs.io/): A Javascript testing framework focused on simplicity.

## How to Run the Application

Skip the installer sections if you already have the following installed:
- Node
- NPM

### Manual Installation on macOS & Windows

1. Install Node

To install Node 22 on Windows or macOS, follow these steps:

- Visit the Node website at https://nodejs.org/en/download.
- Ensure the LTS (long-term support) tab is selected.
- Click on the installer relevant to your computer (pkg for macOS, msi for Windows).
- Follow the installation wizard with the default settings.

2. Verify the installation

- Open a terminal/command line.
- Type `node -v`.
  - You should see `v22.X.X`.

### Package Installation

You can also install Node using a package manager:
- Chocolatey: Windows
- Homebrew: macOS
- Various package managers for Linux: https://nodejs.org/en/download/package-manager

This approach can be more complex if issues arise and requires more terminal/command line experience. If using WSL on Windows, you can use Linux package managers depending on the distribution installed on WSL.

### Node Version Manager

Another approach for installing Node is using Node Version Manager (nvm). This is available for Linux or macOS. Follow the guide [here](https://github.com/nvm-sh/nvm).

Once installed, you can simply type:

```
nvm install 22
nvm use 22
```

## Running the Typescript Application

To run the Typescript service using Node & npm, follow these steps:

- Navigate to the root directory of your Typescript application in the terminal/command line. For this repository, the command would be:
  - `cd typescript`

1. Install the service dependencies: `npm install`
2. Start the application: `npm start`
  - The app should now be available at: `http://localhost:4567`
3. Execute the unit tests: `npm test`