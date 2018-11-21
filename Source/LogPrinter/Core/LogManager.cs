namespace LogPrinter
{
    /// <summary>
    /// manager the log system
    /// </summary>
    public class LogManager
    {
        public LogFormat Format { get; set; }
        public LogPrint Print { get; set; }

        public LogManager()
        {
            Format = new LogFormat();
            Print = new LogPrint();
        }
        
        public void Log(string logText, params object[] context)
        {
            var log = Format.GenerateLog(logText, context);
            
            Print.ApplyLog(log);
        }
    }
}