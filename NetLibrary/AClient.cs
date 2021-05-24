using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;

namespace NetLibrary
{
    public class AClient
    {
        // событие на получение фрейма, который и возвращает
        public delegate void ReceiveEvent(AFrame frame);
        public event ReceiveEvent Receive;

        // адрес группы
        public IPAddress GroupIPAdress; // = IPAddress.Parse("224.0.0.0");
        // локальный хост
        public IPAddress localIPAdress;
        // порт для отправки данных
        public int remotePort; //= 8000;    
        // поток для отлавливания сообщений
        Thread Receiver;
        UdpClient receiver;

        public bool InReceive { get => DoLoop; }

        private bool DoLoop = true;

        public AClient(string adress, int port)
        {
            GroupIPAdress = IPAddress.Parse(adress);
            remotePort = port;
            localIPAdress = IPAddress.Parse(LocalIPAddress());
        }

        public void StartReceive(string name)
        {
            //Receiver = new Thread(ReceiveFrame) { Name = name, IsBackground = true };
            //Receiver.Start();
            // UdpClient для получения данных
            receiver = new UdpClient(remotePort);
            // подключаемся к группе 
            receiver.JoinMulticastGroup(GroupIPAdress, localIPAdress);
            receiver.Client.ReceiveTimeout = 5;
        }

        public void StopReceive()
        {
            DoLoop = false;
            //if ((receiver is null) == false)
            //{
                receiver?.Close();
            //}
            Receiver = null;
        }

        // главная функция по принятию сообщений из сети
        public void ReceiveFrame()
        {
            IPEndPoint remoteIp = null;
            string localAddress = LocalIPAddress();
            try
            {
                // получаем данные
                byte[] data = receiver.Receive(ref remoteIp);
                // если будешь тестить программу на одном пк - закомментированное ниже - не трожь, иначе сообщения не будут отлавливаться
                /*if (remoteIp.Address.ToString().Equals(localAddress))
                    continue;*/
                // вызываем событие о получении сообщения
                AFrame frame = ByteArrayToObject(data) as AFrame;
                Receive?.Invoke(frame);
            }
            // получаем сообщение об ошибке
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //receiver.Close();
            }
        }

        // все что ниже - чекай класс AServer - идентично

        public string LocalIPAddress()
        {
            string localIP = "";
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
    }

}
