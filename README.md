# advertiser-system
Advertising system for newspaper (a fictional newspaper that is) developed as assigment on a course at Umeå university, Sweden. Allows users to create ads and perform CRUD-operations on these.

Users are divided into two categories: "subscribers" and "companies". Subscribers are alllowed to publish ads for free provided they are registered and has a subscriber number. Companies do not have to be pre-registered but are required to enter their company number to publish an add (which incurs a small fee). Details of the company publishing the add are saved to a database automatically.  

Developed using ASP.NET Core MVC. Uses mssql-databases for storing details on ads, subscribers and companies. Custom-built DAL:s provide accsess to the databases and handles the CRUD-operations on it's tables.

Project "Subscribers" basically contains information (in a mssql-database) on registered subscribers and DAL methods to extract this information. It works as rest-api and an Api-controller that produces JSON is responsible for routing and Get- and Put-operations on the Subscriber database.

Project "Advertisers" contains the core functionality of the advertiser system such as views and controller for routing. In addition it conatins the databases with information on companies and ads and DAL-methods to extract this information. Operations requiring Subscriber information are done via Api-calls to the Subscriber-project.

Folder "sql-scripts" contains the sql-queries that were used to create database tables and procedures for CRUD-operations.

Please note that visual design was not a requirement for this assigment. The layout of the views are therefore just standard bootstrap and not pretty at all.

The language of the advertiser-system is Swedish. 
