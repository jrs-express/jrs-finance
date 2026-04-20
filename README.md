# JRS Express Accounting System (Blazor + EF Core + MSSQL)

This solution provides a multi-branch accounting system scaffold aligned to BIR CAS reporting requirements.

## Tech Stack
- Blazor Server (.NET 8)
- EF Core SQL Server (code-first)
- MSSQL connection: `Server=LAPTOP-9NVM87DO\\SQLEXPRESS;Database=JrsExpressAccounting;Trusted_Connection=True;TrustServerCertificate=True;`

## Included Features
- Login + role-based access (Accounting, AccountingAdmin, Admin)
- Dashboard
- Branches, Chart of Accounts (+ sub accounts), Banks
- Customers/Vendors with BIR-required details
- Journal Entries with RVAT and WTAX fields
- Income deposits and cash disbursements/withdrawals
- Check printing queue
- General Ledger + Income Statement
- BIR CAS summary report page with filters
- Admin users and roles page
- Full audit logging on data modifications
- Pagination/filtering scaffolds on growth-prone lists
- Seed data for initial roles/users/master records

## Seed Credentials
- `admin` / `Admin@123`
- `accounting` / `Accounting@123`

## Typical Commands (run inside project folder)
```bash
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```
