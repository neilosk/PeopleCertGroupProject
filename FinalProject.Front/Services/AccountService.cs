using FinalProject.Data.Dtos;
using FinalProject.Front.Helpers;
using Microsoft.AspNetCore.Identity.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FinalProject.Front.Services
{
    public class AccountService
    {
        private readonly HttpClient _httpClient;
        private readonly IContextHelper _contextHelper;
        private readonly string _apiBaseUrl = "https://localhost:7193/api/account"; // Adjust the URL as per your backend API

        public AccountService(HttpClient httpClient, IContextHelper contextHelper)
        {
            _httpClient = httpClient;
            _contextHelper = contextHelper;
            _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _contextHelper.Token);
        }

        //Login

        public async Task<LoginResponseDto> LoginAsync(string email, string password)
        {

            var loginRequest = new LoginRequest
            {
                Email = email,
                Password = password
            };



            var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");


            try
            {
                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/login", content);


                if (response.IsSuccessStatusCode)
                {
                    using var responseContent = await response.Content.ReadAsStreamAsync();



                    var result = await JsonSerializer.DeserializeAsync<LoginResponseDto>(responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    result.IsSuccess = true;
                    _contextHelper.Token = result.Token;
                    _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _contextHelper.Token);
                    return result;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return new LoginResponseDto { IsSuccess = false, Message = errorContent };
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it) and return an error response
                return new LoginResponseDto { IsSuccess = false, Message = ex.Message };
            }
        }


        //Logout
        public async Task Logout()
        {
            await _httpClient.PostAsync($"{_apiBaseUrl}/logout", null);
        }

        //Register
        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequest registerRequest)
        {
            var content = new StringContent(JsonSerializer.Serialize(registerRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/register", content);

            if (response.IsSuccessStatusCode)
            {
                using var responseContent = await response.Content.ReadAsStreamAsync();
                var registerResponse = await JsonSerializer.DeserializeAsync<RegisterResponseDto>(responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return registerResponse;
            }
            else
            {
                // Handle non-successful response
                return new RegisterResponseDto { IsSuccess = false, Message = "Registration failed" };
            }
        }

        //Get all accounts
        public async Task<List<UserProfileDto>> GetAccounts()
        {

            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/GetAllUsers");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            using var responseContent = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<List<UserProfileDto>>(responseContent,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        //Get an account by id
        public async Task<UserProfileDto> GetAccount(int id)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            using var responseContent = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<UserProfileDto>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        //Create an account
        public async Task<UserProfileDto> CreateAccount(UserProfileDto account)
        {
            var accountJson = new StringContent(JsonSerializer.Serialize(account), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiBaseUrl, accountJson);
            response.EnsureSuccessStatusCode();
            using var responseContent = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<UserProfileDto>(responseContent,
                                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        //Update an account
        public async Task UpdateAccount(UserProfileDto account)
        {
            var accountJson = new StringContent(JsonSerializer.Serialize(account), Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"{_apiBaseUrl}/{account.Id}", accountJson);
        }

        //Delete an account
        public async Task DeleteAccount(int id)
        {
            await _httpClient.DeleteAsync($"{_apiBaseUrl}/{id}");
        }
    }
}
