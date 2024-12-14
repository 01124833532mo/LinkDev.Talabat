#E-commerce of Talabat API
## Project Overview
This API provides endpoints for:
-Managing users and user authentication
-Retrieving and filtering products by type and brand
-Adding products to a temporary basket
-Creating and confirming orders
-Integrating Stripe for secure payment processing
-Dashboard to manged Products , Brand , Roles,Users by Admin
-Using Identity Package (Authentcation & Authorization)
## Technologies Used
-ASP.NET Core: For building the API.

-Entity Framework Core: For ORM and database management.

-SQL Server: As the primary database to store persistent data.

-Redis: For temporary basket storage.

-Stripe API: For payment processing with intent-based transactions.

-AutoMapper: For mapping database models to data transfer objects (DTOs).

-Genaric Repository : is a design pattern commonly used in implementing the repository pattern. It allows developers to create reusable, type-safe, and scalable data access layers by using generic types.

-Specification Design Pattern : is a behavioral design pattern that allows you to encapsulate business rules or criteria in a reusable and combinable way. It is often used to create complex business rules or filter criteria in a clean and manageable way, avoiding code duplication.

-Cashing : Caching is a technique used to store frequently accessed data in a temporary storage area, or "cache," to improve application performance and reduce the load on databases or APIs

-Onion Architecture:is a software architecture pattern that emphasizes maintainability, scalability, and testability. It was introduced by Jeffrey Palermo and is designed to overcome the limitations of traditional layered architectures by focusing on the separation of concerns and dependency inversion principles.
 
 -Unite Of Work :is a design pattern that maintains a list of objects affected by a business transaction and coordinates the writing out of changes and resolving concurrency problems. It acts as a bridge between the business logic layer and the data access layer, ensuring that all database operations in a single transaction either succeed or fail together.
 
-Service Manager : is a design pattern or architectural concept used to manage and coordinate services in an application. It acts as a central hub to access, instantiate, and maintain services, promoting a cleaner and more organized code structure. It’s commonly used in larger applications to reduce tight coupling between components and simplify service dependencies.
 
 -Error Handling : the process of identifying, managing, and resolving errors or exceptions that occur during the execution of a program. Effective error handling ensures that your application remains robust, user-friendly, and secure, even when unexpected issues arise.
 
- JWT : s a powerful tool for stateless authentication and secure data transmission. By following best practices, it can significantly enhance the security and efficiency of your application.

  ## Project Structure
-Users: Stores user data, using Identity for authentication and authorization.
-Products: Stores products, with relationships to Brand and Type.
-Basket: Temporary storage of items for checkout using Redis.
-Orders: Stores order data and integrates with Stripe for payment intent.

## Database Design
-User Table: Stores user information such as email, password, and roles.
-Product Table: Each product has a TypeId and BrandId, referencing the product's type and brand.
-Brand and Type Tables: Each brand and type is defined with an ID and name.
-Basket (in Redis): Temporarily stores products added by users until they confirm an order.
-Order Table: Stores order details once the basket is confirmed and payment is completed.

## Key Features
1. User Management
-Registration and Authentication: Users can sign up and sign in using secure authentication.

-Authorization: Certain endpoints are protected, accessible only to authenticated users.

3. Product Management
    -Temporary Basket: Uses Redis to temporarily store basket items before an order is placed.
    - Product Brands and Types: Provides endpoints to list product brands and types.
4. Basket Management
    -Temporary Basket: Uses Redis to temporarily store basket items before an order is placed.
  - Redis Integration: Redis cache is configured to persist basket data temporarily for improved performance and reduced database load.

4. Order and Payment Processing
    -Order Creation: Once a user confirms their basket, it’s converted into an order.
   -Stripe Payment: Payment is handled using Stripe’s PaymentIntent API, allowing secure transactions.
    -Order Confirmation: If payment is successful, the order is saved in the database and marked as completed.
5. Stripe Integration
   -Payment Intent: The payment flow is handled by creating a PaymentIntent in Stripe for each order, ensuring secure payment handling.
    -Testing: Stripe test cards are used to verify the payment functionality in a development environment.



  








