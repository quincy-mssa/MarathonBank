using System;
using System.IO;
using Microsoft.Maui.Controls;

namespace MarathonBank
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // Check if user already exists
            string userFilePath = "C:\\Users\\quinc\\OneDrive\\Desktop\\UserAccountData.txt";
            if (File.Exists(userFilePath))
            {
                var userLines = File.ReadAllLines(userFilePath);
                foreach (var line in userLines)
                {
                    var parts = line.Split(',');
                    if (parts[0] == username)
                    {
                        await DisplayAlert("Error", "Username already exists.", "OK");
                        return;
                    }
                }
            }

            // Add the new user to UserAccountData.txt
            File.AppendAllText(userFilePath, $"{username},{password}\n");

            // Now add a default Checking account for this user
            string accountFilePath = "C:\\Users\\quinc\\OneDrive\\Desktop\\BankAccountsData.txt";
            File.AppendAllText(accountFilePath, $"{username},Checking,0.00\n");

            await DisplayAlert("Success", "Account created successfully!", "OK");
            await Navigation.PopAsync();  // Go back to login page
        }
    }
}
