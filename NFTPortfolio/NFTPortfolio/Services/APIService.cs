using NFTPortfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace NFTPortfolio.Services
{
    static class APIService
    {
        private const string __APIKEY = "5uyeQ9I2Hk6wRIXIzFh6EsAocTQBqHRM";

        public static HttpClient APIClient { get; set; }

        public static void InitializeClient()
        {
            APIClient = new HttpClient();
            APIClient.BaseAddress = new Uri("https://cardano-mainnet.blockfrost.io/api/v0/");
            APIClient.DefaultRequestHeaders.Accept.Clear();
            APIClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            APIClient.DefaultRequestHeaders.Add("project_id", __APIKEY);
        }

        public static async Task<string> GetStakeAddress(string addr)
        {
            var stakeAddr = await EndpointProcessor<AddressInfoModel>.LoadAsync($"addresses/{addr}");
            return stakeAddr.Stake_Address;
        }

        public static async Task<List<UnitModel>> GetAssets(string stakeAddr, int page = 1)
        {
            return await EndpointProcessor<List<UnitModel>>.LoadAsync($"accounts/{stakeAddr}/addresses/assets?page={page}");
        }

        public static async Task<List<AssetModel>> GetAssetMetadata(List<string> assetHashes)
        {
            SemaphoreSlim throttler = new SemaphoreSlim(20, 20);
            List<AssetModel> responses = new List<AssetModel>(assetHashes.Count);
            int requestCount = 0;

            var tasks = assetHashes.Select(async asset =>
            {
                await throttler.WaitAsync();

                var task = EndpointProcessor<AssetModel>.LoadAsync($"assets/{asset}");
                _ = task.ContinueWith(async s =>
                {
                    if (requestCount > 500) await Task.Delay(650);
                    //Console.WriteLine($"\t\tWAITING - #{assetHashes.IndexOf(asset)}: {asset}");
                    throttler.Release();
                });
                try
                {
                    var result = await task;
                    //Console.WriteLine($"DONE - #{assetHashes.IndexOf(asset)}: {asset}");
                    responses.Add(result);
                    requestCount++;
                }
                catch (Exception e)
                {
                    Console.Write($"\t\t\t {asset} error out - ");
                    Console.WriteLine("Error: " + e);
                }
            });

            await Task.WhenAll(tasks);
            return responses;
        }
    }
}
