using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarathonBank
{
    public static class UserDatabase
    {
        private static readonly string userFilePath = @"C:\Users\quinc\OneDrive\Desktop\BankData.txt";
        private static readonly string accountFilePath = @"C:\Users\quinc\OneDrive\Desktop\UserAccountData.txt";

        public static bool AddUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            if (UserExists(username))
                return false;

            try
            {
                // Save user credentials
                using (StreamWriter writer = new StreamWriter(userFilePath, append: true))
                {
                    writer.WriteLine($"{username},{password}");
                }

                // Initialize account data with default balances
                using (StreamWriter writer = new StreamWriter(accountFilePath, append: true))
                {
                    writer.WriteLine($"{username},Checking:0.00,Savings:0.00,Credit:0.00");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user: {ex.Message}");
                return false;
            }
        }

        public static bool UserExists(string username)
        {
            if (!File.Exists(userFilePath))
                return false;

            var lines = File.ReadAllLines(userFilePath);
            return lines.Any(line => line.Split(',')[0].Trim() == username);
        }
    }
}

