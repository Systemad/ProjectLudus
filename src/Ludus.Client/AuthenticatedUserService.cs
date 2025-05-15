using System.Security.Claims;
using Microsoft.Kiota.Abstractions;

namespace Ludus.Client;

public class AuthenticatedUserService
{
    private IUserApi _userApi;
    public ClaimsPrincipal User { get; set; } = new(new ClaimsIdentity());

    public AuthenticatedUserService(IUserApi userApi)
    {
        _userApi = userApi;
    }

    private UserDto? userModel; // { get; private set; }

    public UserDto? UserModel
    {
        get { return userModel; }
        set { userModel = value; }
    }

    public async Task InitializeAsync()
    {
        Console.WriteLine("InitializeAsync");

        await FetchUserModelAsync();
    }

    public async Task<UserDto?> FetchUserModelAsync()
    {
        try
        {
            var me = await _userApi.Me();
            if (me.IsSuccessful)
                UserModel = me.Content;
            return UserModel;
        }
        catch (ApiException e)
        {
            return null;
        }
    }

    //public bool IsAuthenticated => userModel != null;
    //public Guid UserId => UserModel?.Id ?? Guid.Empty;
    //public bool IsAdmin => UserModel?.Role?.Equals(RoleConstants.AdminRoleId) ?? false;
}
