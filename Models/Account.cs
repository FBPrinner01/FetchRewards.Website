using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FetchRewards.Website.Models
{
    public class Account
    {
        public string ID { get; set; }
        public Transaction[] Transactions { get; set; }

        public override string ToString() => JsonSerializer.Serialize<Account>(this);

    }
}