using System;
using System.Text;
using System.Net.Sockets;

namespace Server
{
    public class ClientThread
    {
        private TcpClient Client { get; set; }

        private NetworkStream stream;

        private LogWorker logWorker;
     
        public ClientThread (TcpClient newTcpClient)
        {
            Client = newTcpClient;
        }
        
        private StringBuilder ReadData()
        {
            StringBuilder builderRet = new StringBuilder();
            byte[] data = new byte[4096];
            do
            {
                int bytes = stream.Read(data, 0, data.Length);
                builderRet.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (stream.DataAvailable);
        
            return builderRet;
        }

        private void SendMsg(string msg)
        {
            byte[] data = Encoding.Unicode.GetBytes(msg);
            stream.Write(data, 0, data.Length);
        }
        
        public void Process()
        {
            stream = null;
            try
            {
                stream = Client.GetStream();
                while (true)
                {
                    string msg = ReadData().ToString();

                    string resultMsg = "completed";
                    
                    Console.Write(msg);
                    if (msg.Contains(".log"))
                    {
                        logWorker = new LogWorker(msg);
                        logWorker.ReadLog();
                        resultMsg = "completed";
                    }

                    if (msg == "GetLogs")
                    {
                        resultMsg = logWorker.GetLogs();
                    }

                    if (msg == "GetWarning")
                    {
                        resultMsg = logWorker.GetWarning();
                    }

                    if (msg.Contains("GetOnIdRange"))
                    {
                        var args = msg.Split(' ');
                        resultMsg = logWorker.GetOnIdRange(Convert.ToInt32((args[1])), Convert.ToInt32(args[2]));
                    }

                    if (msg.Contains("RemoveOnRangeId"))
                    {
                        var args = msg.Split(' ');
                        logWorker.RemoveOnRangeId(Convert.ToInt32((args[1])), Convert.ToInt32(args[2]));
                        resultMsg = "completed";
                    }

                    if (resultMsg != "")
                    {
                        msg = resultMsg;
                        SendMsg(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                stream?.Close();
                Client?.Close();
            }
        }
    }
    
    

}