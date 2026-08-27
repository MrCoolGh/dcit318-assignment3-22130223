using System;

namespace Dcit318Assignment3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("       DCIT 318 - ASSIGNMENT 3");
            Console.WriteLine("==============================================");
            Console.WriteLine();

            try
            {
                // Question 1
                var financeApp = new FinanceApp();
                financeApp.Run();

                // Question 2
                var healthSystemApp = new HealthSystemApp();
                healthSystemApp.Run();

                // Question 3
                var inventoryApp = new InventoryApp();
                inventoryApp.Run();

                // Question 4
                var studentInformationSystem = new StudentInformationSystem();
                studentInformationSystem.Run();

                // Question 5
                var paymentSystemApp = new PaymentSystemApp();
                paymentSystemApp.Run();

                Console.WriteLine("==============================================");
                Console.WriteLine("All questions completed successfully.");
                Console.WriteLine("==============================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("An unexpected error occurred:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
