using System.Net;
using System.Net.Http.Json;
using Ludus.Client.Models;
using Ludus.Shared;

namespace Ludus.Client;

public class AuthenticatedUserService
{
    private readonly HttpClient _httpClient;

    public AuthenticatedUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public User User { get; private set; }

    public async Task InitializeAsync()
    {
        await UpdateUserInfoAsync();
    }

    private async Task UpdateUserInfoAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/api/user/me");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var userResponse = await response.Content.ReadFromJsonAsync<User>();
            User = userResponse;
        }
    }

    public bool IsAuthenticated => User != null;
    public Guid UserId => User?.Id ?? Guid.Empty;
    public bool IsAdmin => User?.Role?.Equals(RoleConstants.AdminRoleId) ?? false;
}
