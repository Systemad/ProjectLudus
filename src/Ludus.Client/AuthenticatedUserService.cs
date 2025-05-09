using System.Net;
using System.Net.Http.Json;
using Ludus.Client.Models;
using Ludus.Shared;
using Microsoft.Kiota.Abstractions;

namespace Ludus.Client;

public class AuthenticatedUserService
{
    private readonly HttpClient _httpClient;
    private LudusClient _client;

    public AuthenticatedUserService(HttpClient httpClient, LudusClient client)
    {
        _httpClient = httpClient;
        _client = client;
    }

    public UserDto User { get; private set; }

    public async Task InitializeAsync()
    {
        await UpdateUserInfoAsync();
    }

    private async Task UpdateUserInfoAsync()
    {
        try
        {
            var me = await _client.Api.User.Me.GetAsync();
            User = me;
        }
        catch (ApiException e)
        {
            if (e.ResponseStatusCode == 401) { }
        }
        /*
        //var me = await _client.Api.User.Me.GetAsync();
        if (User is not null)
        {
            //User = me;
        }
        HttpResponseMessage response = await _httpClient.GetAsync("/api/user/me");
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var userResponse = await response.Content.ReadFromJsonAsync<User>();
            User = userResponse;
        }*/
    }

    public bool IsAuthenticated => User != null;
    public Guid UserId => User?.Id ?? Guid.Empty;
    public bool IsAdmin => User?.Role?.Equals(RoleConstants.AdminRoleId) ?? false;
}
