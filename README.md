# Warranty Registration App

**Description**: 
This application when finished will allow companies to process warranty registration from remote locations or integrate it into their existing web applications with RESTFUL API.

## Project Outline:
1. Created the base. Make sure you run your migrations
2. Connect to an actual database instead of SQL file
3. Create the API empty shell
4. Add unit testing capabilities for the function or methods calls
5. Will consume the C# application with either React, Vue or Angular. I haven't chosen which one yet or I may do it for all of them
7. Maybe host it on an actual server either on AWS or Azure and make it available to the public

### Things done today because i had to work on a new computer:
Installed SQL server express and SQL server managment studio (SSMS) on local machine
installed ASP.NET Core Hosting Bundle
Installed and Setup IIS server on my local machine
Deployed to IIS server and Connnect application to SQL server


### Running Migrations
To add-Migrations
Add-Migration DataModels -context WarrantyDataContext

And Update Database
Update-Database -context WarrantyDataContext