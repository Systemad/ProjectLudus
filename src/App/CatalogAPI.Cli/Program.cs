using System.CommandLine;
using CatalogAPI.Cli.Docker;
using Ductus.FluentDocker.Builders;

class Program
{
    static int Main(string[] args)
    {
        RootCommand rootCommand = new RootCommand();
        Option<string> postgresVersionOption = new Option<string>("--postgres")
        {
            Description = "Postgres version",
            DefaultValueFactory = _ => DefaultParameters.POSTGRES_VERSION,
        };
        Option<string> pgSearchVersionOption = new Option<string>("--pgsearch")
        {
            Description = "pg_search version",
            DefaultValueFactory = _ => DefaultParameters.PG_SEARCH_TAG,
        };

        Option<string> pgmqVersionOption = new Option<string>("--pgmq")
        {
            Description = "pgmq version",
            DefaultValueFactory = _ => DefaultParameters.PGMQ_VERSION,
        };

        Command dockerFactoryCommand = new("build", "build custom docker image")
        {
            postgresVersionOption,
            pgSearchVersionOption,
            pgmqVersionOption,
        };

        rootCommand.Subcommands.Add(dockerFactoryCommand);

        rootCommand.SetAction(parseResults =>
            DockerFactory.BuildDockerImage(
                parseResults.GetValue(postgresVersionOption),
                parseResults.GetValue(pgSearchVersionOption),
                parseResults.GetValue(pgmqVersionOption)
            )
        );

        Console.WriteLine("executing");
        return rootCommand.Parse(args).Invoke();
    }
}
