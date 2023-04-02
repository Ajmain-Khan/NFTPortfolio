using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NFTPortfolio.Services
{
    class EndpointProcessor<T>
    {
        public static async Task<T> LoadAsync(string url)
        {
            using (HttpResponseMessage response = await APIService.APIClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    T result = await response.Content.ReadAsAsync<T>();
                    return result;
                }
                else
                {
                    Console.WriteLine("Error: {0} ({1})\n", response.StatusCode, response.ReasonPhrase);
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
