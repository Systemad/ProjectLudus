using Microsoft.Extensions.DependencyInjection;

namespace AppHost.Commands;


public static class SetupCommandExtensions
{
    public static IResourceBuilder<PostgresDatabaseResource> WithSetupCommand(
        this IResourceBuilder<PostgresDatabaseResource> builder)
    {
        var commandOptions = new CommandOptions()
        {
            UpdateState = OnUpdateResourceState,
            IconName = "",
            IconVariant = IconVariant.Filled
        };

        builder.WithCommand(name: "fetch-data", displayName: "Fetch data from IGDB",
            executeCommand: context => OnRunClearCacheCommandAsync(builder, context), commandOptions: commandOptions);
        return builder;
    }

    private static async Task<ExecuteCommandResult> OnRunClearCacheCommandAsync(
        IResourceBuilder<PostgresDatabaseResource> builder,
        ExecuteCommandContext context)
    {
        //var prov = context.ServiceProvider.GetRequiredService<IDataFetcherService>();
        return CommandResults.Success();
    }


    private static ResourceCommandState OnUpdateResourceState(
        UpdateCommandStateContext context)
    {

        return ResourceCommandState.Enabled;
    }
}