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
    public class AServer
    {// событие на отправку фрейма, возвращает статус отправки
        public delegate void AfterSendEvent(bool status);
        public event AfterSendEvent AfterSend;

        private delegate void OnCloseEvent();
        private event OnCloseEvent CloseEvent;

        // адрес группы
        public IPAddress GroupIPAdress;
        // локальный хост
        public IPAddress localIPAdress;
        // порт для отправки данных
        public int remotePort;
        // поток для отправки данных в сеть
        private Thread Sender;
        UdpClient sender;
        public bool InSend = false;
        private bool DoLoop = true;

        public AServer(string adress, int port)
        {
            GroupIPAdress = IPAddress.Parse(adress);
            remotePort = port;
            localIPAdress = IPAddress.Parse(LocalIPAddress());
        }

        // запускаем поток отправки данных в сеть
        public void StartSending(AFrame frame, bool OneSending, string name)
        {
            DoLoop = !OneSending;
            if ((Sender is null) == false)
            {
                StopSending();
            }
            InSend = true;
            Sender = new Thread(new ParameterizedThreadStart(SendFrame)) { Name = name, IsBackground = true };
            Sender.Start(frame);
        }

        // соответственно, останавливаем отправку
        public void StopSending()
        {
            if ((sender is null) == false)
            {
                sender.Close();
            }
            Sender = null;
            InSend = false;
            DoLoop = false;
            //CloseEvent?.Invoke();
        }

        // главная функция для отправки данных в общую сеть
        // используется технология широковещания, при которой данные отсылаются определенной группе
        public void SendFrame(object message)
        {
            // создаем UdpClient для отправки
            sender = new UdpClient();
            //sender.Client.SendTimeout = 50;
            IPEndPoint endPoint = new IPEndPoint(GroupIPAdress, remotePort);
            try
            {
                //while (DoLoop == true)
                {
                    byte[] data = ObjectToByteArray(message);
                    // отправка
                    sender.Send(data, data.Length, endPoint);
                    // вызываем событие того, что фрейм был отправлен удачно (особой роли не играет, используется для дебага)
                    AfterSend?.Invoke(true);
                }
                /*if (DoLoop == false)
                {
                    byte[] data = ObjectToByteArray(message);
                    // отправка
                    sender.Send(data, data.Length, endPoint);
                    // вызываем событие того, что фрейм был отправлен удачно (особой роли не играет, используется для дебага)
                    AfterSend?.Invoke(true);
                }*/
            }
            // в случае каких-либо косяков получаем ошибку
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // опять же - вызываем событие об не удачной отправке сообщения
                AfterSend?.Invoke(false);
            }
            finally
            {
                //sender.Close();
            }
        }

        // определяем локальный адрес ПК (все, что написано ниже - ваще хз, работает и работает)
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

        // функции для преобразования фрейма в байт-массив
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
