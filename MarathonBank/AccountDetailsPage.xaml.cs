using System;
using System.IO;
using Microsoft.Maui.Controls;

namespace MarathonBank
{
    public partial class AccountDetailsPage : ContentPage
    {
        public Account Account { get; private set; }
        private readonly string filePath = "C:\\Users\\quinc\\OneDrive\\Desktop\\The Marathon Accounts.txt";

        public AccountDetailsPage(Account account)
        {
            InitializeComponent();
            Account = account;
            BindingContext = this;
        }

        private void UpdateAccountFile()
        {
            var accounts = File.ReadAllLines(filePath);
            for (int i = 0; i < accounts.Length; i++)
            {
                var parts = accounts[i].Split(',');
                if (parts[0] == Account.AccountNumber)
                {
                    accounts[i] = $"{Account.AccountNumber},{Account.AccountType},{Account.Balance}";
                    break;
                }
            }
            File.WriteAllLines(filePath, accounts);
        }

        private void OnDepositClicked(object sender, EventArgs e)
        {
            if (decimal.TryParse(AmountEntry.Text, out decimal amount) && amount > 0)
            {
                Account.Balance += amount;
                UpdateAccountFile();
                OnPropertyChanged(nameof(Account));
            }
        }

        private void OnWithdrawClicked(object sender, EventArgs e)
        {
            if (decimal.TryParse(AmountEntry.Text, out decimal amount) && amount > 0 && Account.Balance >= amount)
            {
                Account.Balance -= amount;
                UpdateAccountFile();
                OnPropertyChanged(nameof(Account));
            }
            else
            {
                DisplayAlert("Error", "Invalid amount or insufficient funds", "OK");
            }
        }
    }
}
