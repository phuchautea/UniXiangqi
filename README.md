# UniXiangqi

A real-time online Chinese Chess (Xiangqi) platform built with ASP.NET Core 7.0, SignalR, and Entity Framework Core. Players can create rooms, invite opponents, and play Xiangqi matches with full move validation and turn-based logic.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
  - [Step 1 - Clone the Repository](#step-1---clone-the-repository)
  - [Step 2 - Configure the Database Connection](#step-2---configure-the-database-connection)
  - [Step 3 - Apply Database Migrations](#step-3---apply-database-migrations)
  - [Step 4 - Run the Application](#step-4---run-the-application)
  - [Step 5 - Access the Application](#step-5---access-the-application)
- [API Endpoints](#api-endpoints)
- [SignalR Game Hub](#signalr-game-hub)
- [Game Pieces](#game-pieces)
- [Database Schema](#database-schema)
- [Tech Stack](#tech-stack)

## Prerequisites

Before running this project, make sure you have the following installed:

1. **.NET 7.0 SDK** - Download from [https://dotnet.microsoft.com/download/dotnet/7.0](https://dotnet.microsoft.com/download/dotnet/7.0)
2. **SQL Server** - SQL Server Express or SQL Server LocalDB (included with Visual Studio)
3. **EF Core CLI Tools** - Install globally by running:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

## Project Structure

The solution follows **Clean Architecture** with 4 layers:

```
UniXiangqi.sln
|
|-- UniXiangqi.API              # Web API layer (Controllers, SignalR Hubs, Config)
|   |-- Controllers/
|   |   |-- UsersController         # Register, Login, Get User Info
|   |   |-- RoomsController         # Create, List, Get rooms
|   |   |-- MatchesController       # Create, Update status, Get matches
|   |   |-- PieceMovesController    # Create, List, Get move history
|   |-- Hubs/
|   |   |-- GameHub                 # Real-time game logic via SignalR
|   |-- Program.cs                  # App entry point & service registration
|
|-- UniXiangqi.Application     # Application layer (Interfaces & DTOs)
|   |-- Interfaces/                 # Service contracts (IUserService, IRoomService, etc.)
|   |-- DTOs/                       # Data Transfer Objects (User, Room, Match, PieceMove)
|
|-- UniXiangqi.Domain          # Domain layer (Entities, Enums, Game Logic)
|   |-- Entities/
|   |   |-- Room                    # Room with host, opponent, timers, password
|   |   |-- Match                   # Match with players, status, turns
|   |   |-- PieceMove               # Move history with board state snapshots
|   |   |-- UserInRoom              # Viewer/spectator tracking
|   |   |-- Game/
|   |       |-- Board               # Board state management (9x10 grid)
|   |       |-- Pieces/             # Move validation for each piece type
|   |-- Enums/
|   |   |-- MatchStatus             # pending, playing, completed
|   |   |-- PieceType               # Advisor, Cannon, Chariot, Elephant, General, Horse, Soldier
|   |-- Identity/
|       |-- ApplicationUser         # Extended ASP.NET Identity user
|
|-- UniXiangqi.Infrastructure  # Infrastructure layer (EF Core, Services)
|   |-- Persistence/
|   |   |-- ApplicationDbContext    # EF Core DbContext with Identity
|   |-- Services/                   # Service implementations
|   |-- Migrations/                 # EF Core database migrations
```

## Getting Started

### Step 1 - Clone the Repository

```bash
git clone https://github.com/phuchautea/UniXiangqi.git
cd UniXiangqi
```

### Step 2 - Configure the Database Connection

Open `UniXiangqi.API/appsettings.json` and update the `DefaultConnection` string to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=YOUR_SERVER_NAME\\SQLEXPRESS;Database=UniXiangqi;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

**Common connection string examples:**

| SQL Server Type | Connection String |
|---|---|
| SQL Server Express | `Data Source=.\SQLEXPRESS;Database=UniXiangqi;Integrated Security=True;TrustServerCertificate=True;` |
| LocalDB | `Data Source=(localdb)\MSSQLLocalDB;Database=UniXiangqi;Integrated Security=True;TrustServerCertificate=True;` |
| Named Instance | `Data Source=YOUR_PC_NAME\SQLEXPRESS;Database=UniXiangqi;Integrated Security=True;TrustServerCertificate=True;` |

### Step 3 - Apply Database Migrations

Run the following command from the solution root directory to create the database and apply all migrations:

```bash
dotnet ef database update --project UniXiangqi.Infrastructure --startup-project UniXiangqi.API
```

This will create the `UniXiangqi` database with the following tables:
- ASP.NET Identity tables (AspNetUsers, AspNetRoles, etc.)
- Rooms
- Matches
- PieceMoves
- UserInRooms

### Step 4 - Run the Application

**Option A: Using the .NET CLI**

```bash
dotnet run --project UniXiangqi.API
```

**Option B: Using Visual Studio**

1. Open `UniXiangqi.sln` in Visual Studio 2022 or later.
2. Set `UniXiangqi.API` as the startup project.
3. Press `F5` to run with debugging, or `Ctrl+F5` to run without debugging.

### Step 5 - Access the Application

Once the application is running:

| Resource | URL |
|---|---|
| Swagger UI (API docs) | `https://localhost:{port}/swagger` |
| SignalR Game Hub | `https://localhost:{port}/hubs/game` |

The Swagger UI is only available in the Development environment. The exact port will be shown in the console output when the application starts.

## API Endpoints

### Users (`/api/Users`)

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Users/register` | Register a new user account |
| POST | `/api/Users/login` | Login and receive a JWT token |
| GET | `/api/Users/info` | Get current user info (requires JWT in header) |

### Rooms (`/api/Rooms`)

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Rooms` | Create a new game room |
| GET | `/api/Rooms` | Get all rooms |
| GET | `/api/Rooms/{roomCode}` | Get a room by its code |
| GET | `/api/Rooms/user/{userId}` | Get rooms by user ID |

### Matches (`/api/Matches`)

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Matches` | Create a new match |
| POST | `/api/Matches/updateStatus` | Update match status |
| GET | `/api/Matches/{matchId}` | Get a match by ID |
| GET | `/api/Matches/rooms/{roomCode}` | Get a match by room code |
| GET | `/api/Matches/infoMatch/{matchId}` | Get detailed match info |

### Piece Moves (`/api/PieceMoves`)

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/PieceMoves` | Record a new piece move |
| GET | `/api/PieceMoves` | Get all piece moves |
| GET | `/api/PieceMoves/{matchId}` | Get piece moves by match ID |
| GET | `/api/PieceMoves/GetLastestByRoomCode/{roomCode}` | Get the latest move in a room |

## SignalR Game Hub

The real-time gameplay is managed through a SignalR Hub at `/hubs/game`.

**Connection:** Connect with query parameters `roomCode` and `jwt`:
```
wss://localhost:{port}/hubs/game?roomCode=ABC1234&jwt=your_token_here
```

**How it works:**

1. When a player connects, the hub checks if they are the host or opponent of the room.
2. If the room has no opponent yet, the connecting player is automatically assigned as the opponent.
3. Additional users join as viewers/spectators.
4. When both players are connected, the game can be created with the default board setup.
5. Moves are validated server-side using piece-specific movement rules before being broadcast.

**Events sent to clients:**

| Event | Description |
|---|---|
| `ReceiveBoard` | Receives the current board state string |
| `ReceiveMovePiece` | Receives move coordinates (fromRow, fromCol, toRow, toCol) |
| `ReceiveMessage` | Receives chat/system messages |
| `ReceiveSuccessAlert` | Receives success notifications |
| `ReceiveErrorAlert` | Receives error notifications |

## Game Pieces

The board uses a 9x10 grid. Each piece is encoded as a 2-character string: `{Side}{Type}`.

| Code | Piece | Vietnamese |
|---|---|---|
| V | General (King) | Tuong/Vuong |
| S | Advisor | Si |
| T | Elephant | Tuong/Tinh |
| M | Horse | Ma |
| X | Chariot (Rook) | Xe |
| P | Cannon | Phao |
| C | Soldier (Pawn) | Tot/Chot |

Sides: `R` = Red, `B` = Black. Example: `RX` = Red Chariot, `BC` = Black Soldier.

## Database Schema

```
AspNetUsers (ASP.NET Identity)
  |-- Id, UserName, Email, TotalPoint, ...
  
Rooms
  |-- Id, Code (7-char random), GameTimer, MoveTimer
  |-- HostUserId -> AspNetUsers
  |-- OpponentUserId -> AspNetUsers
  |-- HostSide, IsRated, Password, IsRedTurn, IsHostTurn
  
Matches
  |-- Id, RoomId -> Rooms, RoomCode
  |-- RedUserId, BlackUserId, WinnerUserId
  |-- StartTime, EndTime, Turn, NextTurn
  |-- MatchStatus (pending, playing, completed)
  
PieceMoves
  |-- Id, MatchId -> Matches, RoomCode
  |-- PlayerUserId -> AspNetUsers
  |-- Side (R/B), MoveContent (chess notation), ChessBoard (full board snapshot)
  
UserInRooms
  |-- Id, UserId -> AspNetUsers, RoomId -> Rooms, IsPlayer
```

## Tech Stack

| Component | Technology |
|---|---|
| Framework | ASP.NET Core 7.0 |
| Real-time Communication | SignalR |
| ORM | Entity Framework Core 7.0 |
| Database | SQL Server |
| Authentication | ASP.NET Identity + JWT |
| API Documentation | Swagger / Swashbuckle |