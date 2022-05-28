namespace File.Replayer
{
    using System;
    using System.IO;
    using System.Text;

    public class TraceLog : IDisposable
    {
        private StringBuilder sb = new StringBuilder();

        public TraceLog()
        {
           
        }

        public void Log(string message)
        {
            Console.WriteLine(message);
            sb.AppendLine(message);
        }

        public void Dispose()
        {
            var path = Directory.GetCurrentDirectory();
            using (StreamWriter sw = new StreamWriter(Path.Combine(path, $"tracelog_{Guid.NewGuid()}.txt")))
            {
                sw.WriteLine(sb.ToString());
                sb = null;
                sw.Close();
            }
        }
    }
}
