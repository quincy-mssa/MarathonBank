using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarathonBank
{
    public static class AccountDatabase
    {
        private static readonly string accountFilePath = @"C:\Users\quinc\OneDrive\Desktop\UserAccountData.txt";

        public static Dictionary<string, double> GetAccountBalances(string username)
        {
            if (!File.Exists(accountFilePath))
                return null;

            var lines = File.ReadAllLines(accountFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');

                if (parts.Length >= 4 && parts[0].Trim() == username)
                {
                    return new Dictionary<string, double>
                {
                    { "Checking", double.Parse(parts[1].Split(':')[1]) },
                    { "Savings", double.Parse(parts[2].Split(':')[1]) },
                    { "Credit", double.Parse(parts[3].Split(':')[1]) }
                };
                }
            }

            return null; // User not found
        }

        public static bool UpdateAccountBalance(string username, string accountType, double newBalance)
        {
            if (!File.Exists(accountFilePath))
                return false;

            var lines = File.ReadAllLines(accountFilePath);
            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(',');

                if (parts[0].Trim() == username)
                {
                    if (accountType == "Checking")
                        parts[1] = $"Checking:{newBalance:F2}";
                    else if (accountType == "Savings")
                        parts[2] = $"Savings:{newBalance:F2}";
                    else if (accountType == "Credit")
                        parts[3] = $"Credit:{newBalance:F2}";

                    lines[i] = string.Join(",", parts);
                    File.WriteAllLines(accountFilePath, lines);
                    return true;
                }
            }

            return false; // User not found
        }
    }

}
