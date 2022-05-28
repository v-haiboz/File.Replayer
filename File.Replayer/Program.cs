namespace File.Replayer
{
    using CommandLine;
    using System.Threading.Tasks;

    internal class Program
    {
        [Verb("DepotEvent", HelpText = "Replay DepotEvent")]
        public class DepotEventOptions
        {
            [Option('n', "configurationFile", Required = true, HelpText = "Replayer Name Configuration File Path(CSV).")]
            public string ReplayerNameFilePath { get; set; }

            [Option('c', "configurationFile", Required = true, HelpText = "Replayer Content Configuration File Path(txt).")]
            public string ReplayerContentFilePath { get; set; }

            [Option('o', "configurationFile", Required = true, HelpText = "Replayer OutPut Dir Path.")]
            public string ReplayerOutPutDirPath { get; set; }
        }

        static async Task<int> Main(string[] args)
        {
#if DEBUG
            args = new string[] { @"-n", @"C:\Work\SourceCode\File.Replayer\File.Replayer\Examples\ReplayerName.csv",
                                  @"-c", @"C:\Work\SourceCode\File.Replayer\File.Replayer\Examples\ReplayerContent.txt",
                                  @"-o", @"C:\Work\SourceCode\File.Replayer\File.Replayer\Examples"};
#endif
            return await Parser.Default.ParseArguments<DepotEventOptions>(args)
              .MapResult(
                (DepotEventOptions opts) => ReplayDepotEvent(opts),
                errs => Task.FromResult<int>(1));
        }

        static async Task<int> ReplayDepotEvent(DepotEventOptions opts)
        {
            var instance = new FileReplayerManager(opts);
            return await instance.Replay();
        }
    }
}
