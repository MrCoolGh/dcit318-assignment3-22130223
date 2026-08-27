using System;
using System.Collections.Generic;

namespace Dcit318Assignment3
{
    // Q1(a): Record for transaction data
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // Q1(b): Interface for processing behavior
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // Q1(c): Concrete processors
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Bank Transfer] Processed amount: {transaction.Amount:C} for category: {transaction.Category}");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Mobile Money] Processed amount: {transaction.Amount:C} for category: {transaction.Category}");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Crypto Wallet] Processed amount: {transaction.Amount:C} for category: {transaction.Category}");
        }
    }

    // Q1(d): Base account class
    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
        }
    }

    // Q1(e): Sealed specialized account
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance)
        {
        }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
                return;
            }

            base.ApplyTransaction(transaction);
            Console.WriteLine($"Transaction applied. Updated balance: {Balance:C}");
        }
    }

    // Q1(f): App simulation
    public class FinanceApp
    {
        private readonly List<Transaction> _transactions = new();

        public void Run()
        {
            Console.WriteLine("=== QUESTION 1: Finance Management System ===");

            SavingsAccount account = new("SAV-001", 1000m);

            Transaction t1 = new(1, DateTime.Now, 150m, "Groceries");
            Transaction t2 = new(2, DateTime.Now, 220m, "Utilities");
            Transaction t3 = new(3, DateTime.Now, 120m, "Entertainment");

            ITransactionProcessor mobileProcessor = new MobileMoneyProcessor();
            ITransactionProcessor bankProcessor = new BankTransferProcessor();
            ITransactionProcessor cryptoProcessor = new CryptoWalletProcessor();

            mobileProcessor.Process(t1);
            account.ApplyTransaction(t1);

            bankProcessor.Process(t2);
            account.ApplyTransaction(t2);

            cryptoProcessor.Process(t3);
            account.ApplyTransaction(t3);

            _transactions.Add(t1);
            _transactions.Add(t2);
            _transactions.Add(t3);

            Console.WriteLine($"Final balance for account {account.AccountNumber}: {account.Balance:C}");
            Console.WriteLine();
        }
    }
}
