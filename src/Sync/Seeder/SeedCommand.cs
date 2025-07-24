using System.ComponentModel;
using Spectre.Console.Cli;

namespace Seeder;

public sealed class SeedOptions : CommandSettings
{
    [Description("Path to search. Defaults to current directory.")]
    [CommandArgument(0, "[searchPath]")]
    public string? SearchPath { get; init; }

    [CommandOption("--caching")]
    [DefaultValue(true)]
    public bool Caching { get; init; }
}

public class SeedCommand : Command<SeedOptions>
{
    public override int Execute(CommandContext context, SeedOptions settings)
    {
        throw new NotImplementedException();
    }
}