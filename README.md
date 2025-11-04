# ShopEasy - Microservices Project

A simple .NET microservices demonstration project with two microservices that communicate with each other.

## Architecture

This project consists of two microservices:

1. **NameInputService** - Handles name input from users
2. **NameDisplayService** - Displays the name received from NameInputService

## Services Overview

### NameInputService (Port: 5255)
- **Purpose**: Accepts name input from users via a web interface
- **API Endpoint**: `POST /api/submitname`
- **UI**: Accessible at `http://localhost:5255`
- **Functionality**: 
  - Provides a simple HTML form to enter a name
  - On button click, sends the name to NameDisplayService via HTTP POST

### NameDisplayService (Port: 5010)
- **Purpose**: Receives and displays names from NameInputService
- **API Endpoints**: 
  - `POST /api/displayname` - Receives name from InputService
  - `GET /api/getname` - Returns the current name
- **UI**: Accessible at `http://localhost:5010`
- **Functionality**:
  - Receives names from NameInputService
  - Displays the name on a web page with auto-refresh capability

## Prerequisites

- .NET 8.0 SDK or later
- A web browser

## How to Run

1. **Restore dependencies** (optional, should be done automatically):
   ```bash
   dotnet restore
   ```

2. **Run both services**:

   You need to run both services in separate terminal windows:

   **Terminal 1 - NameInputService:**
   ```bash
   cd NameInputService
   dotnet run
   ```
   The service will start at `http://localhost:5255`

   **Terminal 2 - NameDisplayService:**
   ```bash
   cd NameDisplayService
   dotnet run
   ```
   The service will start at `http://localhost:5010`

3. **Access the applications**:
   - Open `http://localhost:5255` in your browser for the Name Input Service
   - Open `http://localhost:5010` in another tab for the Name Display Service

## How to Use

1. Open the NameInputService page (`http://localhost:5255`)
2. Enter a name in the input field
3. Click the "Send to Display Service" button
4. Switch to the NameDisplayService page (`http://localhost:5010`)
5. The name should appear automatically (or click the "Refresh" button)

## Technology Stack

- **.NET 8.0** - Framework
- **ASP.NET Core** - Web API framework
- **Minimal APIs** - API endpoints
- **HTML/CSS/JavaScript** - Frontend UI
- **HTTP Client** - Inter-service communication

## Project Structure

```
ShopEasy/
├── ShopEasy.sln              # Solution file
├── NameInputService/          # First microservice
│   ├── Program.cs            # Main application code
│   ├── wwwroot/
│   │   └── index.html        # UI for input service
│   └── appsettings.json      # Configuration
└── NameDisplayService/        # Second microservice
    ├── Program.cs            # Main application code
    ├── wwwroot/
    │   └── index.html        # UI for display service
    └── appsettings.json      # Configuration
```

## Configuration

The communication URL between services can be configured in `NameInputService/appsettings.json`:

```json
{
  "DisplayServiceUrl": "http://localhost:5010"
}
```

## API Documentation

Once the services are running, Swagger UI is available at:
- NameInputService: `http://localhost:5255/swagger`
- NameDisplayService: `http://localhost:5010/swagger`

## Notes

- Both services use CORS to allow cross-origin requests
- Static files are enabled for serving the HTML pages
- The DisplayService uses in-memory storage (restart will clear the name)
- For production use, consider adding authentication, proper error handling, and persistent storage

## Docker

### Build and Run with Docker Compose

Prerequisites: Docker Desktop installed and running.

1. Build images and start both services:
   ```bash
   docker compose up --build
   ```

2. Open the apps:
   - NameInputService: `http://localhost:5255`
   - NameDisplayService: `http://localhost:5010`

3. Stop and remove containers:
   ```bash
   docker compose down
   ```

Notes:
- In containers, services listen on port 8080; host ports map to 5255 and 5010.
- `NameInputService` reads `DisplayServiceUrl` from environment; compose sets it to `http://namedisplayservice:8080` for inter-container calls.
