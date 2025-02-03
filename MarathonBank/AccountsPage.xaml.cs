using System;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Maui.Controls;
using System.Linq;

namespace MarathonBank
{
    public partial class AccountsPage : ContentPage
    {
        public ObservableCollection<Account> Accounts { get; set; } = new();

        private readonly string filePath = "C:\\Users\\quinc\\OneDrive\\Desktop\\BankAccountsData.txt";

        public AccountsPage()
        {
            InitializeComponent();
            LoadAccounts();
            BindingContext = this;
        }

        private void LoadAccounts()
        {
            string loggedInUser = Preferences.Get("LoggedInUser", string.Empty);

            if (string.IsNullOrEmpty(loggedInUser))
            {
                DisplayAlert("Error", "No user logged in.", "OK");
                return;
            }

            Accounts.Clear();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 3 && parts[0] == loggedInUser) // Load only logged-in user's accounts
                    {
                        Accounts.Add(new Account
                        {
                            AccountNumber = parts[0],
                            AccountType = parts[1],
                            Balance = decimal.TryParse(parts[2], out decimal balance) ? balance : 0
                        });
                    }
                }
            }
        }

        private void DepositFunds(decimal amount)
        {
            string loggedInUser = Preferences.Get("LoggedInUser", string.Empty);

            if (string.IsNullOrEmpty(loggedInUser))
            {
                DisplayAlert("Error", "No user logged in.", "OK");
                return;
            }

            // Read all accounts
            var lines = File.ReadAllLines(filePath).ToList();
            bool updated = false;

            for (int i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split(',');
                if (parts.Length == 3 && parts[0] == loggedInUser && parts[1] == "Checking")
                {
                    // Update balance
                    if (decimal.TryParse(parts[2], out decimal currentBalance))
                    {
                        decimal newBalance = currentBalance + amount;
                        lines[i] = $"{loggedInUser},Checking,{newBalance:F2}";
                        updated = true;
                        break;
                    }
                }
            }

            if (updated)
            {
                File.WriteAllLines(filePath, lines);
                DisplayAlert("Success", $"${amount:F2} added to your checking account!", "OK");
                LoadAccounts(); // Refresh account display
            }
            else
            {
                DisplayAlert("Error", "Checking account not found.", "OK");
            }
        }

        private void WithdrawFunds(decimal amount)
        {
            string loggedInUser = Preferences.Get("LoggedInUser", string.Empty);

            if (string.IsNullOrEmpty(loggedInUser))
            {
                DisplayAlert("Error", "No user logged in.", "OK");
                return;
            }

            // Read all accounts
            var lines = File.ReadAllLines(filePath).ToList();
            bool updated = false;

            for (int i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split(',');
                if (parts.Length == 3 && parts[0] == loggedInUser && parts[1] == "Checking")
                {
                    // Update balance
                    if (decimal.TryParse(parts[2], out decimal currentBalance))
                    {
                        if (currentBalance >= amount)
                        {
                            decimal newBalance = currentBalance - amount;
                            lines[i] = $"{loggedInUser},Checking,{newBalance:F2}";
                            updated = true;
                            break;
                        }
                        else
                        {
                            DisplayAlert("Error", "Insufficient funds.", "OK");
                            return;
                        }
                    }
                }
            }

            if (updated)
            {
                File.WriteAllLines(filePath, lines);
                DisplayAlert("Success", $"${amount:F2} withdrawn from your checking account!", "OK");
                LoadAccounts(); // Refresh account display
            }
            else
            {
                DisplayAlert("Error", "Checking account not found.", "OK");
            }
        }

        private void OnDepositClicked(object sender, EventArgs e)
        {
            if (decimal.TryParse(DepositAmountEntry.Text, out decimal amount) && amount > 0)
            {
                DepositFunds(amount);
            }
            else
            {
                DisplayAlert("Error", "Enter a valid deposit amount.", "OK");
            }
        }

        private void OnWithdrawClicked(object sender, EventArgs e)
        {
            if (decimal.TryParse(WithdrawAmountEntry.Text, out decimal amount) && amount > 0)
            {
                WithdrawFunds(amount);
            }
            else
            {
                DisplayAlert("Error", "Enter a valid withdrawal amount.", "OK");
            }
        }
    }
}
