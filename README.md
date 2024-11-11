# Sales  API

## Purpose
This API is used for managing sales orders, products, and providing a dashboard for sales analysis. It supports features like JWT authentication, real-time updates with SignalR, and logging.
It also has swagger documentation. Clean Architecture is used and CQRS Pattern for clear segregation of commands and queries

## Setup
1. Install PostgresSql and configure the connection string
2. Update the connection string in the appsetting of the Test also.
### Prerequisites
- .NET 8 SDK or higher
- Docker (optional)
- PostgresSql

### Running the API
1. Clone the repository.
2. Run the application locally using the following command:

```bash
dotnet run
```

3a. To run the Test:
```
dotnet test
```
or you use the test explorer

4. For Authentication, use Email: `admin@admin.com` and Password: `Admin@12345`
