using System;
using System.Text;
using System.Net.Sockets;


namespace Client
{
    public class ClientModel
    {
        private int Port { get; set; }
        private string Address { get; set; }
        private NetworkStream Stream { get; set; }
        private TcpClient Client { get; set; }
        
        public string UserName { get; set; }
        
        //конструктор 
        public ClientModel()
        {
            Port = 0;
            Address = "";
            Client = null;
            Stream = null;
        }
        
        private void ReadPortAddressAndUserName()
        {
            Console.Write("Port: ");
            Port = Convert.ToInt32(Console.ReadLine());
            Console.Write("Address: ");
            Address = Console.ReadLine();
            Console.Write("Your user name: ");
            UserName = Console.ReadLine();
        }
        private void Connect()
        {
            try
            {
                //создаем нового TCP Clienta с нужным адрисом и портом 
                Client = new TcpClient(Address, Port);
                //создаем поток для подключения к серверу
                Stream = Client.GetStream();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //если все хорошо выводим сообщение что смогли подключится 
            if (Stream != null)
            {
                Console.WriteLine("completed");
            }
        }

        private string ReadRequest()
        {
            Console.Write(UserName + ": ");
            string message = Console.ReadLine();
            return message;
        }

        private void SendMsg(string msg)
        {
            byte[] data = Encoding.Unicode.GetBytes(msg);
            //отправка сообщения
            Stream.Write(data, 0, data.Length);
        }

        private StringBuilder ReceiveMsg()
        {
            //для получения больших данных мы читаем их по блоками 4096 байт 
            StringBuilder builderRet = new StringBuilder();
            int bytes = 0;
            byte[] data = new byte[4096];
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builderRet.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (Stream.DataAvailable);
            return builderRet;
        }

        public void StartClient()
        {
            ReadPortAddressAndUserName();
            Connect();
            while (true)
            {
                //прочитать сообщение от клиента 
                string msg = ReadRequest();
                //отправить сообщение на сервер
                SendMsg(msg);
                //ждем ответ от сервера 
                StringBuilder serverMsg = ReceiveMsg();
                //completed 
                if (serverMsg.ToString() != "completed")
                {
                    Console.WriteLine(serverMsg.ToString());
                }
            }
        }
    }
}