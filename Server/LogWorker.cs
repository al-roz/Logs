using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Server
{
    public class LogWorker
    {
        struct LogTemplate
        {
            public string data { get; set; }
            public string messageType { get; set; }
            
            public string anotherInfo { get; set; }
            
            public long logID { get; set; }
        }
        private string filePath;

        
        
        private List<LogTemplate> logs = new List<LogTemplate>();

        public LogWorker(string filePath)
        {
            this.filePath = filePath;
        }

        public void ReadLog()
        {
            string[] logsLine = File.ReadAllLines(filePath);
            foreach (var i in logsLine)
            {
                
                LogTemplate newLog = new LogTemplate();
                newLog.logID = 0;
                newLog.data = i.Substring(0, i.IndexOf(','));
                var x = i.IndexOf(' ', newLog.data.Length);
                newLog.messageType = i.Substring(newLog.data.Length + 2, i.IndexOf(' ', newLog.data.Length + 2) - 2 - newLog.data.Length);
                newLog.anotherInfo = i.Substring(newLog.data.Length + newLog.messageType.Length + 3);
                newLog.messageType.Replace(" ",String.Empty);
                logs.Add(newLog);
                
                
            }

            var s = logs.ToArray();
            for (int i = s.Length - 1; i >= 0; i--)
            {
                s[i].logID = s.Length - i + 1;
            }

            logs = s.ToList();
        }

        public string GetLogs()
        {
            StringBuilder result = new StringBuilder();
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                result.Append(logs[i].logID + "::" + logs[i].data + "\t" + logs[i].messageType + logs[i].anotherInfo + "\n\n");
            }

            return result.ToString();
        }

        public string GetWarning()
        {
            StringBuilder result = new StringBuilder();
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                if (logs[i].messageType == "Warning")
                    result.Append(logs[i].logID + "::" + logs[i].data + "\t" + logs[i].messageType + logs[i].anotherInfo + "\n\n");
            }
            return result.ToString();
        }

        public string GetOnIdRange(long idStart, long idEnd)
        {
            StringBuilder result = new StringBuilder();
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                if (logs[i].logID >= idStart && logs[i].logID <= idEnd)  
                    result.Append(logs[i].logID + "::" + logs[i].data + "\t" + logs[i].messageType + logs[i].anotherInfo + "\n\n");
            }
            return result.ToString();
        }

        public void RemoveOnRangeId(long idStart, long idEnd)
        {
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                if (logs[i].logID >= idStart && logs[i].logID <= idEnd)
                {
                    logs.Remove(logs[i]);
                }
            }
        }
    }
}