using System.Net;
using System.Net.Http.Json;
using Shared;

namespace Client;

public class AuthenticatedUserService
{
    private readonly HttpClient _httpClient;

    public AuthenticatedUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public LudusUser UserInfo { get; private set; }

    public async Task InitializeAsync()
    {
        await UpdateUserInfoAsync();
    }

    private async Task UpdateUserInfoAsync()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:7211/");
        
        //_httpClient.BaseAddress = new Uri("https://localhost:7211/");
        HttpResponseMessage response = await client.GetAsync("/api/me");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var hey  = await response.Content.ReadFromJsonAsync<LudusUser>();
            UserInfo = hey;
        }
    }

    public bool IsAuthenticated => true; //UserInfo != null;
    public int UserId => UserInfo?.Id ?? 0;
    //public bool IsAdmin => UserInfo?.Role?.Equals(RoleConstants.AdminRoleId) ?? false;
}