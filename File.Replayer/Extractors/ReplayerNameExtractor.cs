namespace File.Replayer.Extractors
{
    using File.Replayer.Models;
    using System;
    using System.IO;

    internal class ReplayerNameExtractor: ExtractorBase<ReplayerContent>
    {

        ///<param name="filePath">the file path</param>
        ///<remarks> For detailed format, you can refer to the documents here: \File.Replayer\Examples\ReplayerName.csv
        ///the given CSV is like:
        ///<para>
        ///8B93B15C-E349-4E29-9DD9-426778DE2B75,
        ///8B93B15C-E349-4E29-9DD9-426778DE2B75,
        ///5498B2BC-6113-4247-A63C-446730B4F5A3,
        ///5498B2BC-6113-4247-A63C-446730B4F5A3,
        ///9AF5EB2B-A52D-4E9D-BD34-3E81B6129A95,
        ///</para>
        ///</remarks>
        protected override void Travel(string filePath)
        {
            var encoding = GetType(filePath);
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs, encoding);
            string strLine = "";
            while ((strLine = sr.ReadLine()) != null)
            {
                Analysis(strLine);
            }

            sr.Close();
            fs.Close();
        }

        ///<summary>
        /// The incoming line is like:
        /// <para>
        /// 5498B2BC-6113-4247-A63C-446730B4F5A3
        /// </para>
        ///</summary>
        private void Analysis(string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            var array = line.Split(',');
            if (array.Length != 1)
            {
                Console.WriteLine($"Error:This row of data is not in the right format:{line}");
                return;
            }

            //will be added to the HashSet only once.
            var obj = new ReplayerContent()
            {
                Name = array[0],
                 SourcePath = filePath
            };

            Add(obj);
        }
    }
}
