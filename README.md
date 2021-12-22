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
Installed SQL server express and SQL server management studio (SSMS) on local machine

installed ASP.NET Core Hosting Bundle

Installed and Setup IIS server on my local machine

Deployed to IIS server and connected application to SQL server


### Running Migrations
To add-Migrations
- Add-Migration DataModels -context WarrantyDataContext

To Update Database
- Update-Database -context WarrantyDataContext

The unit test for the project can be found here https://github.com/cirsam/WarrantyRegUnitTest



## User stories

As an Employee, I want to add a warranty so that the customer can receive service

 
GIVEN a non-warrantied product

WHEN valid warranty details are submitted

THEN the product will be warrantied for 5 years from the purchase date

 

GIVEN a non-warrantied product

WHEN invalid warranty details including a non-existent customer id are submitted

THEN the product will not be warrantied

 

GIVEN a non-warrantied product

WHEN invalid warranty details including a non-existent serial number are submitted

THEN the product will not be warrantied

 

GIVEN a non-warrantied product

WHEN invalid warranty details including a non-unique serial number are submitted

THEN the product will not be warrantied



As a Employee, I want to extend a warranty so that the customer can continue to receive service

GIVEN an existing product warranty

WHEN the warranty is extended

THEN the expiration date is extended by 2 years