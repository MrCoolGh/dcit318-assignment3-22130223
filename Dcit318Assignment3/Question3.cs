using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dcit318Assignment3
{
    // Q3(a): Marker interface
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // Q3(b): Product record
    public record Product(int Id, string Name, int Quantity, decimal Price) : IInventoryEntity;

    // Q3(c): Generic inventory logger
    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private readonly string _logFilePath;

        public InventoryLogger(string logFilePath = "inventory_log.txt")
        {
            _logFilePath = logFilePath;
        }

        public void Add(T item)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Added item with ID: {item.Id}";
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }

        public List<string> GetAllLogs()
        {
            if (!File.Exists(_logFilePath))
            {
                return new List<string>();
            }

            return File.ReadAllLines(_logFilePath).ToList();
        }
    }

    // Q3(d): Inventory exception
    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message) : base(message)
        {
        }
    }

    // Q3(e): Inventory app
    public class InventoryApp
    {
        private readonly List<Product> _products = new();
        private readonly InventoryLogger<Product> _logger = new();

        public void SeedSampleData()
        {
            _products.Add(new Product(1, "Laptop", 10, 5200m));
            _products.Add(new Product(2, "Mouse", 0, 120m)); // intentionally zero to trigger exception
            _products.Add(new Product(3, "Keyboard", 15, 300m));
        }

        public void ProcessInventory()
        {
            Console.WriteLine("=== QUESTION 3: Warehouse Inventory Management ===");

            foreach (var product in _products)
            {
                try
                {
                    if (product.Quantity <= 0)
                    {
                        throw new InvalidQuantityException($"Product '{product.Name}' has invalid quantity: {product.Quantity}");
                    }

                    _logger.Add(product);
                    Console.WriteLine($"Processed product: {product.Name} (Qty: {product.Quantity}, Price: {product.Price:C})");
                }
                catch (InvalidQuantityException ex)
                {
                    Console.WriteLine($"Inventory Error: {ex.Message}");
                }
                finally
                {
                    Console.WriteLine($"Finished processing product ID {product.Id}");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Inventory log contents:");
            foreach (var line in _logger.GetAllLogs())
            {
                Console.WriteLine(line);
            }

            Console.WriteLine();
        }

        public void Run()
        {
            SeedSampleData();
            ProcessInventory();
        }
    }
}
