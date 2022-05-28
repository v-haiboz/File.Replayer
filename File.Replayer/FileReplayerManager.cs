namespace File.Replayer
{
    using File.Replayer.Extractors;
    using System;
    using System.Threading.Tasks;
    using static File.Replayer.Program;
    using System.IO;

    internal class FileReplayerManager
    {
        private DepotEventOptions depotEventOptions;
        public FileReplayerManager(DepotEventOptions depotEventOptions)
        {
            this.depotEventOptions = depotEventOptions;
        }

        public async Task<int> Replay()
        {
            using (var trace = new TraceLog())
            {
                trace.Log($"Start:{DateTime.UtcNow} -> Replayer File");
                var replayerFileNameList = new ReplayerNameExtractor().Extract(depotEventOptions.ReplayerNameFilePath);

                var replayerFileContent = new ReplayerContentExtractor().Extract(depotEventOptions.ReplayerContentFilePath);

                foreach (var file in replayerFileNameList)
                {
                    var dirPath = Path.Combine(depotEventOptions.ReplayerOutPutDirPath, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    var filePath = Path.Combine(dirPath, file.Name);
                    if (string.IsNullOrEmpty(Path.GetExtension(filePath)))
                    {
                        filePath = Path.ChangeExtension(filePath, ".txt");
                    }
                    foreach (var item in replayerFileContent)
                    {
                        File.WriteAllText(filePath, item.Content);
                        trace.Log($"    {filePath}->{DateTime.UtcNow}");
                    }
                }
                trace.Log($"End:{DateTime.UtcNow} -> Replayer File");
                return await Task.FromResult(0);
            }
        }
    }
}
