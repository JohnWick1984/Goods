using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=eugene1984;Initial Catalog=Goods;Integrated Security=True;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Подключение к базе данных успешно.");

                DisplayAllProducts(connection);
                DisplayAllProductTypes(connection);
                DisplayAllSuppliers(connection);

                DisplayProductWithMaxQuantity(connection);
                DisplayProductWithMinQuantity(connection);
                DisplayProductWithMinCost(connection);
                DisplayProductWithMaxCost(connection);

                DisplayProductsByCategory(connection, "Electronics");
                DisplayProductsBySupplier(connection, "TechMart");

                DisplayOldestProduct(connection);
                DisplayAverageQuantityByType(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при подключении к базе данных: " + ex.Message);
            }
        }

        Console.WriteLine("Нажмите любую клавишу для завершения...");
        Console.ReadKey();
    }

    static void DisplayAllProducts(SqlConnection connection)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SELECT * FROM Products", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nИнформация о товарах:");

                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ProductID"]}, Name: {reader["Name"]}, TypeID: {reader["ProductTypeID"]}, SupplierID: {reader["SupplierID"]}, Quantity: {reader["Quantity"]}, CostPrice: {reader["CostPrice"]}, DeliveryDate: {reader["DeliveryDate"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при отображении товаров: " + ex.Message);
        }
    }

    static void DisplayAllProductTypes(SqlConnection connection)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SELECT * FROM ProductTypes", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nВсе типы товаров:");

                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ProductTypeID"]}, Name: {reader["Name"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при отображении типов товаров: " + ex.Message);
        }
    }

    static void DisplayAllSuppliers(SqlConnection connection)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SELECT * FROM Suppliers", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nВсе поставщики:");

                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["SupplierID"]}, Name: {reader["Name"]}, Address: {reader["Address"]}, Phone: {reader["Phone"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при отображении поставщиков: " + ex.Message);
        }
    }

    static void DisplayProductWithMaxQuantity(SqlConnection connection)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM Products ORDER BY Quantity DESC", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nТовар с максимальным количеством:");

                    if (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ProductID"]}, Name: {reader["Name"]}, Quantity: {reader["Quantity"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при отображении товара с максимальным количеством: " + ex.Message);
        }
    }

    static void DisplayProductWithMinQuantity(SqlConnection connection)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM Products ORDER BY Quantity ASC", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nТовар с минимальным количеством:");

                    if (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ProductID"]}, Name: {reader["Name"]}, Quantity: {reader["Quantity"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при отображении товара с минимальным количеством: " + ex.Message);
        }
    }

    static void DisplayProductWithMinCost(SqlConnection connection)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM Products ORDER BY CostPrice ASC", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nТовар с минимальной себестоимостью:");

                    if (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ProductID"]}, Name: {reader["Name"]}, CostPrice: {reader["CostPrice"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при отображении товара с минимальной себестоимостью: " + ex.Message);
        }
    }

    static void DisplayProductWithMaxCost(SqlConnection connection)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM Products ORDER BY CostPrice DESC", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nТовар с максимальной себестоимостью:");

                    if (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ProductID"]}, Name: {reader["Name"]}, CostPrice: {reader["CostPrice"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при отображении товара с максимальной себестоимостью: " + ex.Message);
        }
    }

    static void DisplayProductsByCategory(SqlConnection connection, string category)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE ProductTypeID IN (SELECT ProductTypeID FROM ProductTypes WHERE Name = @Category)", connection))
            {
                command.Parameters.AddWithValue("@Category", category);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine($"\nТовары категории '{category}':");

                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ProductID"]}, Name: {reader["Name"]}, TypeID: {reader["ProductTypeID"]}, Quantity: {reader["Quantity"]}, CostPrice: {reader["CostPrice"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отображении товаров категории '{category}': " + ex.Message);
        }
    }

    static void DisplayProductsBySupplier(SqlConnection connection, string supplier)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE SupplierID IN (SELECT SupplierID FROM Suppliers WHERE Name = @Supplier)", connection))
            {
                command.Parameters.AddWithValue("@Supplier", supplier);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine($"\nТовары поставщика '{supplier}':");

                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ProductID"]}, Name: {reader["Name"]}, SupplierID: {reader["SupplierID"]}, Quantity: {reader["Quantity"]}, CostPrice: {reader["CostPrice"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при отображении товаров поставщика '{supplier}': " + ex.Message);
        }
    }

    static void DisplayOldestProduct(SqlConnection connection)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM Products ORDER BY DeliveryDate ASC", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nСамый старый товар на складе:");

                    if (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["ProductID"]}, Name: {reader["Name"]}, DeliveryDate: {reader["DeliveryDate"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при отображении самого старого товара: " + ex.Message);
        }
    }

    static void DisplayAverageQuantityByType(SqlConnection connection)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SELECT ProductTypeID, AVG(Quantity) AS AvgQuantity FROM Products GROUP BY ProductTypeID", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nСреднее количество товаров по каждому типу:");

                    while (reader.Read())
                    {
                        Console.WriteLine($"ProductTypeID: {reader["ProductTypeID"]}, AvgQuantity: {reader["AvgQuantity"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при отображении среднего количества товаров по каждому типу: " + ex.Message);
        }
    }
}
