using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SignalRServer.Entities;

namespace SignalRServer
{
    class ApiClient
    {
        private readonly HttpClient _client = new HttpClient();

        public ApiClient()
        {

            _client.BaseAddress = new Uri("https://localhost:44380");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
           
        }

        public async Task<Uri> CreateProductAsync(PlayerWrap player)
        {
          
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/players", player);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response.Headers.Location;
        }
    }
}