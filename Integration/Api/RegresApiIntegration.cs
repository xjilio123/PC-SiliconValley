using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiliconValley.Integration.Regres.dto;

namespace SiliconValley.Integration.Regres
{
    public class RegresApiIntegration
    {
        private readonly ILogger<RegresApiIntegration> _logger;
        private const string API_URL = "https://reqres.in/api/users";

        public RegresApiIntegration(ILogger<RegresApiIntegration> logger)
        {
            _logger = logger;
        }

        public async Task<List<Users>> GetAll()
        {
            string requestUrl = $"{API_URL}";
            List<Users> listUsers = new List<Users>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(json);                    
                    var usersArray = jsonObject["data"].ToObject<List<Users>>();
                     return usersArray;
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
                }
            }
            return listUsers;
        }
        public async Task<Users?> GetUserById(int? userId)
            {
                string requestUrl = $"{API_URL}/{userId}"; 
                Users? user = null;
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(requestUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var jsonObject = JObject.Parse(json);
                            user = jsonObject["data"].ToObject<Users>(); 
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
                    }
                }
                return user;
            }
            public async Task<String> CreateUser(Users newUser)
        {
                String? msj="";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        var jsonContent = JsonConvert.SerializeObject(newUser);
                        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync(API_URL, content);

                    }
                    catch (HttpRequestException ex)
                    {
                        _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
                    }
                }
                return msj;

        }
    }
}