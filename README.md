Inventory Management System
A robust inventory tracking solution built with ASP.NET Core and Entity Framework Core.

Features
Product catalog with categories
Supplier management
Inventory transactions (purchases/sales)
Real-time stock level tracking
CSV reporting
Technologies
.NET 8
Entity Framework Core 8
SQL Server
xUnit (testing)
Getting Started
Prerequisites
.NET 8 SDK
SQL Server
Installation
Clone the repository:

git clone https://github.com/atabekyan98/inventory-management-system
Configure the database connection in appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=InventoryDB;Trusted_Connection=True;"
}
Apply database migrations:

dotnet ef database update --project src/Inventory.Infrastructure
Run the application:

dotnet run --project src/Inventory.API
API Endpoints
Method	Endpoint	Description
GET	/api/products	List all products
POST	/api/products	Create new product
GET	/api/products/{id}	Get product details
PUT	/api/products/{id}	Update product
DELETE	/api/products/{id}	Delete product
POST	/api/transactions/purchase	Record inventory purchase
POST	/api/transactions/sale	Record inventory sale
Testing
Run unit tests:

dotnet test tests/Inventory.Core.UnitTests
Run integration tests:

dotnet test tests/Inventory.API.IntegrationTests
License
This project is licensed under the MIT License.
