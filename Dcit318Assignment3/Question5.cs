using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Dcit318Assignment3
{
    // Q5(a): Generic stack implementation
    public class MyStack<T>
    {
        private readonly List<T> _items = new();

        public void Push(T item)
        {
            _items.Add(item);
        }

        public T Pop()
        {
            if (_items.Count == 0)
            {
                throw new InvalidOperationException("Cannot pop from an empty stack.");
            }

            int lastIndex = _items.Count - 1;
            T item = _items[lastIndex];
            _items.RemoveAt(lastIndex);
            return item;
        }

        public int Count => _items.Count;

        public IEnumerable<T> GetAll()
        {
            return new List<T>(_items);
        }
    }

    // Q5(b): Transaction record
    public record PaymentTransaction(int Id, decimal Amount, DateTime Date, string Description);

    // Q5(c): Processor with serialization and exception handling
    public class TransactionProcessor
    {
        private readonly MyStack<PaymentTransaction> _history = new();

        public void ProcessTransaction(PaymentTransaction transaction)
        {
            _history.Push(transaction);
            Console.WriteLine($"Processed transaction: {transaction.Description}, Amount: {transaction.Amount:C}, Date: {transaction.Date:g}");
        }

        public void UndoLastTransaction()
        {
            try
            {
                var removed = _history.Pop();
                Console.WriteLine($"Undid transaction: {removed.Description} ({removed.Amount:C})");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Undo failed: {ex.Message}");
            }
        }

        public void SaveToFile(string path)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_history.GetAll(), options);
                File.WriteAllText(path, json);
                Console.WriteLine($"Transaction history saved to {path}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access error while writing file: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"I/O error while writing file: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while writing file: {ex.Message}");
            }
        }
    }

    // Q5(d): Demo app
    public class PaymentSystemApp
    {
        public void Run()
        {
            Console.WriteLine("=== QUESTION 5: Stack-Based Transaction History ===");

            var processor = new TransactionProcessor();

            processor.ProcessTransaction(new PaymentTransaction(1, 250m, DateTime.Now.AddMinutes(-45), "Tuition payment"));
            processor.ProcessTransaction(new PaymentTransaction(2, 50m, DateTime.Now.AddMinutes(-30), "Printing services"));
            processor.ProcessTransaction(new PaymentTransaction(3, 120m, DateTime.Now.AddMinutes(-10), "Lab fee"));

            processor.UndoLastTransaction();
            processor.UndoLastTransaction();
            processor.UndoLastTransaction();
            processor.UndoLastTransaction(); // demonstrates exception handling on empty stack

            processor.SaveToFile("transactions.json");
            Console.WriteLine();
        }
    }
}
