using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarathonBank
{
    public class Transaction
    {
        public string Username { get; set; }
        public string AccountType { get; set; }
        public string TransactionType { get; set; } // "Deposit" or "Withdrawal"
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
