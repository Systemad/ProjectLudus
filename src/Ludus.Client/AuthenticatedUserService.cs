using System.Net;
using System.Net.Http.Json;
using Ludus.Shared;
using Ludus.Shared.Features.User;

namespace Ludus.Client;

public class AuthenticatedUserService
{
    private readonly HttpClient _httpClient;

    public AuthenticatedUserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public User UserInfo { get; private set; }

    public async Task InitializeAsync()
    {
        await UpdateUserInfoAsync();
    }

    private async Task UpdateUserInfoAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("/api/user/me");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var hey = await response.Content.ReadFromJsonAsync<User>();
            UserInfo = hey;
        }
    }

    public bool IsAuthenticated => UserInfo != null;
    public int UserId => UserInfo?.Id ?? 0;
    public bool IsAdmin => UserInfo?.Role?.Equals(RoleConstants.AdminRoleId) ?? false;
}
