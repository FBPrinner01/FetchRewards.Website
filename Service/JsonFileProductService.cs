using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FetchRewards.Website.Models;
using System;
using System.Globalization;

namespace FetchRewards.Website.Service
{
    public class JsonFileProductService
    {
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }

        private string AccountJson
        {
            //this returns the file path of our user accounts.
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "Accounts.json"); }
        }

        public IEnumerable<Account> GetAccounts()
        {
            //opens our accounts.Json folder sets a Json object with our users and their transactions.
            using (var jsonFileReader = File.OpenText(AccountJson))
            {
                //since we need our object to be Json, deserialize will read the file and make its contents 
                // a json object.
                IEnumerable<Account> Accounts =  JsonSerializer.Deserialize<Account[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                List<Account> OrderedAccount = new List<Account>(); //fill this list with the order based on timestamps
                foreach (var user in Accounts)
                {
                    //assume we have multiple, just in case we need to use this web service down the road.
                    
                    //gets our user, orders their transactions by timestamp, and returns the OrderedAccount list.
                    user.Transactions = OrderTransactions(user.Transactions);
                    OrderedAccount.Add(user);
                }
                return (IEnumerable<Account>)OrderedAccount;
            }
        }
        public Transaction[] OrderTransactions(Transaction[] a)
        {
            //probably didn't need to make this a method, but you can never be too careful nowadays.
            //orders the array and returns it based on timestamps.
            return a.OrderBy(t => t.Timestamp).ToArray();
        }
        public void SpendPoints(int points)
        {
            /*calculate our total points, order the transactions, and begin doing math:
             * if points > transaction.points, subtract transaction.points from points and set transaction.points to 0
             * else, subtract points from transaction.points and set points to 0
             * After that, go through the array and take out any transaction where points is <= 0.
             */ 

            //get transactions and order:
            var accounts = GetAccounts();
            string id = "FBPrinner01";
            var query = accounts.First(x => x.ID == id);
            var transactions = OrderTransactions(query.Transactions.ToArray());

            /*loop through and get the total points before we start. if we don't have enough,
                don't spend the points*/
            var total = 0;
            for (var i = 0; i < transactions.Length; i++)
            {
                total += transactions[i].Points;
            }
            //now that we have total points; begin the purge:
            for (var i = 0; i < transactions.Length; i++)
            {
                if (total < points || points == 0)
                {
                    //break: we cannot continue
                    break;
                }
                else
                {
                    //figure which is more: transaction.points or the spending points and reduce:
                    if (points < transactions[i].Points)
                    {
                        transactions[i].Points -= points;
                        points = 0;
                        total -= points;
                        break; //we don't have any more points to spend, so exit for loopp
                    }
                    else //points > transactions[i].points
                    {
                        points -= transactions[i].Points;
                        total -= transactions[i].Points;
                        transactions[i].Points = 0;
                    }
                }
            }
            //updateTransactions: loop through and add to List<Transactions> newTransactions
            //if Transactions[i].Points == 0, do not add.
            List<Transaction> newTransactions = new List<Transaction>();
            for (var i = 0; i < transactions.Length; i++)
            {
                if (transactions[i].Points > 0)
                {
                    //add to list
                    newTransactions.Add(transactions[i]);
                }
                //else, do nothing
            }
            query.Transactions = newTransactions.ToArray();
            accounts.First(x => x.ID == id).Transactions = query.Transactions;
            File.WriteAllText(AccountJson, JsonSerializer.Serialize<IEnumerable<Account>>(accounts));
        }
        public void AddTransaction(string payer, int points)
        {
            //get accounts, get the transactions, update, and write to file.
            var accounts = GetAccounts();
            string id = "FBPrinner01";
            var query = accounts.First(x => x.ID == id);
            var transactions = query.Transactions.ToList();
            Transaction t = new Transaction(); //this is a temp transaction we use to create new data
            t.Payer = payer;
            t.Points = points;

            //place a watch on timestamp, make sure it outputs as "yyyy-MM-ddTHH:mm:ssZ"
            //make the timestamp. DateTime.Now gives us milliseconds, which we don't want...
            //this probably isn't the cleanest way to do it, but gets us what we want:
            
            DateTime date = DateTime.Now;
            int yyyy = date.Year;
            int MM = date.Month;
            int dd = date.Day;
            int HH = date.Hour;
            int mm = date.Minute;
            int ss = date.Second;

            date = new DateTime(yyyy, MM, dd, HH, mm, ss);
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            
            t.Timestamp = date;
            
            transactions.Add(t);

            //set the first instance of accounts where id = "FBPrinner01" to our new array of transactions
            accounts.First(x => x.ID == id).Transactions = transactions.ToArray();
            using (var outputStream = File.OpenWrite(AccountJson))
            {
                JsonSerializer.Serialize<IEnumerable<Account>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    accounts
                );
            }
        }
    }
}