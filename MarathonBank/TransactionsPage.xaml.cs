using System;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

namespace MarathonBank
{
    public partial class TransactionsPage : ContentPage
    {
        public ObservableCollection<Transaction> Transactions { get; set; } = new();

        private readonly string transactionFilePath = "C:\\Users\\quinc\\OneDrive\\Desktop\\TransactionHistory.txt";

        public TransactionsPage()
        {
            InitializeComponent();
            LoadTransactions();
            BindingContext = this;
        }

        private void LoadTransactions()
        {
            string loggedInUser = Preferences.Get("LoggedInUser", string.Empty);
            if (string.IsNullOrEmpty(loggedInUser)) { DisplayAlert("Error", "No user logged in.", "OK"); return; }

            Transactions.Clear();

            if (File.Exists(transactionFilePath))
            {
                var lines = File.ReadAllLines(transactionFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 5 && parts[0] == loggedInUser)
                    {
                        Transactions.Add(new Transaction
                        {
                            Username = parts[0],
                            AccountType = parts[1],
                            TransactionType = parts[2],
                            Amount = decimal.TryParse(parts[3], out decimal amount) ? amount : 0,
                            Timestamp = DateTime.TryParse(parts[4], out DateTime date) ? date : DateTime.MinValue
                        });
                    }
                }
            }

        }
    }
}
