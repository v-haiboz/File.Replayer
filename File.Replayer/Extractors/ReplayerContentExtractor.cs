namespace File.Replayer.Extractors
{
    using File.Replayer.Models;
    using System;
    using System.IO;

    internal class ReplayerContentExtractor : ExtractorBase<ReplayerName>
    {
        ///<param name="filePath">the file path</param>
        ///<remarks> For detailed format, you can refer to the documents here: \File.Replayer\Examples\ReplayerContent.txt
        ///the given txt is like:
        ///asdfadsfasdf,adfasdfad.q.qwerqwerqwerqwerqwerqwerqwerqwerqwerqwerqwerqwerdfdf.adfasd
        ///adfadfllleeqe
        ///</remarks>
        protected override void Travel(string filePath)
        {
            this.filePath = filePath;
            var encoding = GetType(filePath);
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs, encoding);
            string content = "";
            while (!string.IsNullOrEmpty(content = sr.ReadToEnd()))
            {
                Analysis(content);
            }

            sr.Close();
            fs.Close();
        }

        ///<summary>
        ///The incoming content is like:
        ///<para>
        ///asdfadsfasdf,adfasdfad.q.qwerqwerqwerqwerqwerqwerqwerqwerqwerqwerqwerqwerdfdf.adfasd
        ///adfadfllleeqe
        ///</para>
        ///</summary>
        private void Analysis(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                Console.WriteLine($"Error:This data is null");
                return;
            }

            //will be added to the HashSet only once.
            var obj = new ReplayerName()
            {
                Content = content,
                SourcePath = filePath
            };

            Add(obj);
        }
    }
}
