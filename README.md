# team7-ssis
GDipSA: Stationery Store Inventory System

## Development Guide

1. Clone Git Repository
2. Create new local DB using SSMS `StationeryShop`
3. Open Repository in Visual Studio and Type Enable-Migrations in Package Manager Console, hit enter
4. Type Update-Database in Package Manager Console, hit enter
5. Press `Ctrl-F5` to run the application
6. Register a new user with email `root@admin.com` and password `password`
7. Run `SELECT * FROM AspNetUsers` in SSMS to find UserId of newly registered user
8. Open `StationeryStoreInit.sql` and replace all instances of `[UserId]` with the UserId of the newly registered user
9. Execute SQL statements
10. Run All Tests