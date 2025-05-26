# Inventory Management System

A robust inventory tracking solution built with ASP.NET Core and Entity Framework Core.

## Features

- Product catalog with categories
- Supplier management
- Inventory transactions (purchases/sales)
- CSV/PDF reporting

## Technologies

- .NET 8
- Entity Framework Core 8
- PostgreSQL 
- xUnit (testing)

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/Hak1998/Inventory__Management__System
   ```

2. Configure the database connection in `appsettings.json`:

   ```json
   "ConnectionStrings": {
         "DefaultConnection": "User ID =postgres;Password=YourPassword;Server=localhost;Database=InventoryDB;Pooling=true;"
   }
   ```

3. Apply database migrations:

   ```bash
   dotnet ef database update --project Inventory.Infrastructure
   ```
OR
   ```
   The code will automatically apply any pending EF Core database migrations at runtime — typically during application startup.
   ```

4. Run the application:

   ```bash
   dotnet run --project InvTrack.API
   ```

## API Endpoints

| Method | Endpoint                        | Description              |
|--------|---------------------------------|------------------------- |
| GET    | /api/products                   | List all products        |
| POST   | /api/products                   | Create new product       |
| GET    | /api/products/{id}              | Get product details      |
| PUT    | /api/products/{id}              | Update product           |
| DELETE | /api/products/{id}              | Delete product           |
| POST   | /api/transactions/GetAll        | Get All Transactions     |
| POST   | /api/transactions/purchase      | Record inventory purchase|
| POST   | /api/transactions/sale          | Record inventory sale    |
|        |                                 |                          |



### Testing

- Run unit tests:

  ```bash
  dotnet test tests/InventoryTrack.UnitTests
  ```

- Run integration tests:

  ```bash
  dotnet test tests/InventoryTrack.IntegrationTests
  ```

### License
This project is licensed under the MIT License.
