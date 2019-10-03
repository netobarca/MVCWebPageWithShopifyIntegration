# MVCWebPageWithShopifyIntegration
This is a web application in MVC.NET, it is integrated with Shopify to process some kind of transaction, you have to consider some previous step before to run the application successfully.
1. You have to Change your URL given from Shopify to connect your Application with their API.
2. You have to create a Database with the following script, the password is encrypted but you can access to the OrdersController with this password pass1:

CREATE TABLE BON_USR_USERS (
  idUser int IDENTITY(1,1) PRIMARY KEY,
  email varchar(100) NOT NULL UNIQUE,
  userName varchar(70) NOT NULL UNIQUE,
  password varchar(100) NOT NULL
);

INSERT INTO BON_USR_USERS(EMAIL, USERNAME, PASSWORD)VALUES('user1@gmail.com','user1','e6c3da5b206634d7f3f3586d747ffdb36b5c675757b380c6a5fe5c570c714349');

3. You have to add an new ADO.NET Entity Data Model item with the databases created in the step 2.
4. You have to change the username and the password given from Shopify to connect with their API into the HomeController and OrdersController
5. You have to change the Key given by SendGrid to use their API in the OrdersController.

Hope it will help.
