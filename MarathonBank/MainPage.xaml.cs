using System;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;

namespace MarathonBank
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string enteredUsername = UsernameEntry.Text;
            string enteredPassword = PasswordEntry.Text;

            if (ValidateUser(enteredUsername, enteredPassword)) // Validate username and password
            {
                Preferences.Set("LoggedInUser", enteredUsername);  // Use Preferences instead of App.Current.Properties
                await Navigation.PushAsync(new AccountsPage()); // Navigate to the Accounts page
            }
            else
            {
                await DisplayAlert("Login Failed", "Invalid username or password", "OK");
            }
        }

        private bool ValidateUser(string username, string password)
        {
            string filePath = "C:\\Users\\quinc\\OneDrive\\Desktop\\UserAccountData.txt";

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2 && parts[0] == username && parts[1] == password)  // Match username and password
                    {
                        return true;  // Valid user
                    }
                }
            }
            return false;  // Invalid credentials
        }

        private async void OnCreateAccountClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
