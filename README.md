# BurgerMania_Backend

## Web APIs designed for a user to order burgers from a burger shop. The properly structured backend of the website includes:
1. Models, with Foreign Key Constraints and Validation using Fluent API and Attributes.
2. DTOs, for ease of Data Transfer and to avoid the json-serialization error.
3. Custom Mappers written for Model Object to DTO Object mapping.
4. Custom Controller Logic for all the endpoints required in the website.
5. JWT Authentication and Authorization has been properly configured for secure access.

### This is a personal project to get hands-on experience in C# and Dotnet web-apis. The expected outcome here was for a user to register with username-password, login with the same which would create a token to be furter passed on to other APIs in the header. The user can then add order items in the existing order until he/she places the order, once placed the order with the latest date is marked as 'resolved' instead of 'pending' and a new order is created. An order item corresponds to a single burger item on the menu. 
