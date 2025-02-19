# ASP.NET Ecommerce Application

The ASP.NET Shopping Application is a fully-functional e-commerce platform built with the ASP.NET framework. This project aims to provide users with an intuitive and secure online shopping experience, while also offering robust administrative tools for managing the store. It follows the MVC (Model-View-Controller) architecture for scalability, maintainability, and separation of concerns.

## Features

### For Customers:

-   **Product Catalog**: Users can browse a wide range of products in various categories, view product details, and filter items by attributes like price, brand, and rating.
-   **Shopping Cart**: Customers can easily add, remove, and update the quantity of products in their shopping cart. The cart is saved across sessions.
-   **Order Processing**: A simple, yet secure, checkout process that allows users to review their cart, input shipping details, and finalize orders.
-   **Authentication & Authorization**: Users can create accounts, log in, and track order history.

### For Admins:

-   **Inventory Management**: Admins can add, update, or delete products from the inventory, including details like price, description, and images.
-   **Category Management**: Admins can create and manage product categories to help customers easily navigate the product catalog.
-   **Customer Management**: Admins can view customer details, order history, and manage users (e.g., disable accounts).
-   **Order Management**: Admins can view all orders, update the order status (e.g., shipped, completed), and manage returns or cancellations.

## Technologies Used

-   **ASP.NET Core MVC**: Implements the MVC pattern to separate the user interface, business logic, and data access.
-   **Entity Framework Core**: Used for database management and data models to store and retrieve product, order, and customer information.
-   **SQL Server**: Database backend for storing data.
-   **Bootstrap**: A responsive front-end framework used to build a mobile-friendly and modern user interface.
-   **JavaScript**: Used to enhance interactivity, such as for adding/removing products to/from the shopping cart dynamically.
-   **Authentication**: ASP.NET Identity is used for managing user authentication and authorization.

## Installation

To run this project locally, follow these steps:

1.  **Clone the repository:**
    
    ```bash
    git clone https://github.com/yourusername/aspnet-shopping-app.git
    
    ```
    
2.  **Navigate to the project folder:**
    
    ```bash
    cd aspnet-shopping-app
    
    ```
    
3.  **Restore the NuGet packages:**
    
    ```bash
    dotnet restore
    
    ```
    
4.  **Update the database:**
    
    -   Run the following command to apply migrations:
        
        ```bash
        dotnet ef database update
        ```
        
5.  **Run the application:**
    
    ```bash
    dotnet run
    
    ```
    
    The application will be available at `https://localhost:7001`.
    


Please ensure that your code adheres to the coding style and includes appropriate tests where necessary.

## Contact

If you have any questions or suggestions, feel free to reach out at [thakurnavneet686@gmail.com](mailto:thakurnavneet686@gmail.com) !
