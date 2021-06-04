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
    {

        public const int PackageLength = 16384;

        // событие на отправку фрейма, возвращает статус отправки
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
            //sender.Client.SendTimeout = 5;
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
        }

        // главная функция для отправки данных в общую сеть
        // используется технология широковещания, при которой данные отсылаются определенной группе
        public void SendFrame(object message)
        {
            // создаем UdpClient для отправки
            sender = new UdpClient();
            IPEndPoint endPoint = new IPEndPoint(GroupIPAdress, remotePort);
            try
            {

                byte[] data = ObjectToByteArray(message);
                APackage package = null;

                if (data.Length < PackageLength)
                {
                    package = new APackage(data.Length, data, APackageType.SinglePackage, true);
                    byte[] packageData = ObjectToByteArray(package);
                    sender.Send(packageData, packageData.Length, endPoint);
                }
                else
                {

                    int dLehgth = data.Length;
                    int dCounter = 0;

                    //Console.WriteLine("START SENDING (" + dLehgth + ")");

                    int pCounter = 0;

                    while (dLehgth - dCounter > 0)
                    {
                        Task Sending = new Task(() =>
                        {
                            if (dLehgth - dCounter > PackageLength)
                            {
                                byte[] buffer = new byte[PackageLength];
                                Array.Copy(data, dCounter, buffer, 0, buffer.Length);
                                package = new APackage(data.Length, buffer, APackageType.HugePackage, dCounter.Equals(0) ? true : false);
                                dCounter += PackageLength;
                                pCounter++;
                            }
                            else if (dLehgth - dCounter > 0)
                            {
                                byte[] buffer = new byte[dLehgth - dCounter];
                                Array.Copy(data, dCounter, buffer, 0, dLehgth - dCounter);
                                package = new APackage(data.Length, buffer, APackageType.HugePackage, dCounter.Equals(0)? true: false);
                                dCounter += dLehgth - dCounter;
                                pCounter++;
                            }
                            byte[] packageData = ObjectToByteArray(package);
                            sender.Send(packageData, packageData.Length, endPoint);

                            //Console.WriteLine("SEND: " + dCounter + " / " + data.Length);
                        });

                        Sending.Start();
                        Thread.Sleep(180);
                    }

                    //Console.WriteLine("SEND: Отправленно пакетов " + pCounter + " из " + ((dLehgth / PackageLength) + 1) + " (" + dCounter + " / " + data.Length + ")");

                }

                //byte[] size = ObjectToByteArray(data.Length);

                //int dLehgth = data.Length;
                //int dCounter = 0;

                /*sender.Send(size, size.Length, endPoint);

                while (true)
                {
                    if (dLehgth - dCounter > 4096)
                    {
                        byte[] buffer = new byte[4096];
                        Array.Copy(data, dCounter, buffer, 0, buffer.Length);
                        sender.Send(buffer, buffer.Length, endPoint);
                        dCounter += 4096;
                    }
                    else if (dLehgth - dCounter > 0)
                    {
                        byte[] buffer = new byte[dLehgth - dCounter];
                        Array.Copy(data, dCounter, buffer, 0, buffer.Length);
                        sender.Send(buffer, buffer.Length, endPoint);
                        dCounter += dLehgth - dCounter;
                        break;
                    }
                    else break;
                }*/
            }
            // в случае каких-либо косяков получаем ошибку
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // опять же - вызываем событие об не удачной отправке сообщения
                //AfterSend?.Invoke(false);
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
