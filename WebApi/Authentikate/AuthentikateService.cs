using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace WebApi.Authentikate
{
    public class AuthentikateService : IAuthentikateService
    {
        public AuthentikateService(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public async Task<string> GetUser(HttpContext httpContext)
        {
            var token = parseTorken(httpContext);
            Client.DefaultRequestHeaders.Add("Authorization", token);
            var resposne = await Client.GetAsync("http://localhost:8080/api/user/validate");

            if(!resposne.IsSuccessStatusCode) return null;

            var resposneStream = await resposne.Content.ReadAsStreamAsync();
            using(var reader = new StreamReader(resposneStream))
            using (var jsonTextReader = new JsonTextReader(reader))
            {
                var serializer = new JsonSerializer();
                var auth = serializer.Deserialize<AuthentikateResponse>(jsonTextReader);
                return auth != null && !string.IsNullOrEmpty(auth.Data) ? auth.Data : null;
            }
        }

        private string parseTorken(HttpContext httpContext)
        {
            StringValues authHeader;
            var success = httpContext.Request.Headers.TryGetValue("Authorization", out authHeader);
            return authHeader.ToString();
        }
    }
}