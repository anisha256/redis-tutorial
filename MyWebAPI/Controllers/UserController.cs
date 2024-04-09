using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System;

using MyWebAPI.Models;
using MyWebAPI.Services;
using Newtonsoft.Json;

namespace WebAPI_Pattern_Implementation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IConfiguration _configuration;
        public UserController(HttpClient httpClient, IRedisCacheService redisCacheService, IConfiguration configuration)
        {
            _redisCacheService = redisCacheService;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<ActionResult<Users[]>> Get( CancellationToken ca)
        {
            try
            {
                var cachedUsers = await _redisCacheService.GetCachedDataAsync("customers");

                if (!string.IsNullOrEmpty(cachedUsers))
                {
                    var user = JsonConvert.DeserializeObject<Users[]>(cachedUsers);
                    return Ok(user);
                }
                else
                {

                    var response = await _httpClient.GetAsync(
                        _configuration["api"] + "/users");
                    response.EnsureSuccessStatusCode();
                    var users = await response.Content.ReadFromJsonAsync<Users[]>();

                    await _redisCacheService.SetCachedDataAsync("customers", JsonConvert.SerializeObject(users), TimeSpan.FromMinutes(60));

                    return users;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("failed to fetch users", ex);
            }

        }
    }
}