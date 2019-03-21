namespace CodeArtCommandsExperiment.CodeArtCommand
{
    public interface ICommand
    {
        string[] AllowedRoles { get; }
        string HelpText { get; }
        string Keyword { get; }

        CommandJob CreateJob();
    }
}