using System;
using System.Text.Json;

namespace FetchRewards.Website.Models
{
    public class Transaction
    {
        public string Payer { get; set; }
        public DateTime Timestamp { get; set; }
        public int Points { get; set; }

        public override string ToString() => JsonSerializer.Serialize<Transaction>(this);
    }
}
