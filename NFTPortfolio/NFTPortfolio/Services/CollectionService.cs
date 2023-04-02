using NFTPortfolio.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NFTPortfolio.Services
{
    class CollectionService
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if (db != null)
            {
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "Data.db");
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Collection>();
        }

        public static async Task AddCollection(string name, int quantity, string policy_id)
        {
            await Init();
        }

        public static async Task RemoveCollection(string name, int quantity, string policy_id)
        {
            await Init();
        }

        public static async Task GetCollection(string name, int quantity, string policy_id)
        {
            await Init();
        }

    }
}
