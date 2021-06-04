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
        //Thread Receiver;
        UdpClient receiver;

        public AFrame Result;

        public bool IsComleted;
        private bool _InReceive;
        public bool InReceive => _InReceive;

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
            //receiver.Client.ReceiveTimeout = 1;
            DoLoop = true;
            //receiver.Client.SendBufferSize = receiver.Client.ReceiveBufferSize = 4096;
        }

        public void StopReceive()
        {
            DoLoop = false;
            //if ((receiver is null) == false)
            //{
            receiver?.Close();
            //}
            //Receiver = null;
        }

        public void Reset() => _InReceive = false;

        public void ReceiveResult()
        {

            IsComleted = false;

            IPEndPoint remoteIp = null;
            string localAddress = LocalIPAddress();
            try
            {
                if (receiver is object)
                {

                    //receiver.Client.ReceiveTimeout = 90;
                    // получаем данные
                    _InReceive = false;
                    byte[] data = receiver.Receive(ref remoteIp);
                    if (data is object)
                    {
                        //receiver.Client.ReceiveTimeout = 120;
                        APackage package = ByteArrayToObject(data) as APackage;

                        //Console.WriteLine("START RECEIVE (" + package.Length + ")");

                        if (package.FirstPackage)
                        {
                            byte[] buffer = new byte[package.Length];
                            Array.Copy(package.Data, 0, buffer, 0, package.Data.Length);
                            int dCounter = package.Data.Length;

                            int pCounter = 1;

                            //Console.WriteLine("RECEIVE: " + package.Data.Length + " (" + dCounter + " / " + buffer.Length + ")");

                            if (package.Length > AServer.PackageLength)
                            {
                                while (dCounter < buffer.Length)
                                {
                                    _InReceive = false;
                                    byte[] temp = receiver.Receive(ref remoteIp);
                                    _InReceive = true;
                                    package = ByteArrayToObject(temp) as APackage;
                                    if (package.FirstPackage) break;
                                    Array.Copy(package.Data, 0, buffer, dCounter, package.Data.Length);
                                    dCounter += package.Data.Length;
                                    pCounter++;
                                    //Console.WriteLine("RECEIVE: " + package.Data.Length + " (" + dCounter + " / " + buffer.Length + ")");
                                }
                            }
                            else buffer = package.Data;

                            //Console.WriteLine("RECEIVE: Принято пакетов " + pCounter + " из " + (buffer.Length / AServer.PackageLength) + " (" + dCounter + " / " + buffer.Length + ")");
                            if (package.PackageType.Equals(APackageType.SinglePackage) || !package.FirstPackage)
                            {
                                Result = ByteArrayToObject(buffer) as AFrame;
                                IsComleted = Result is object;
                            }
                            _InReceive = false;
                        }
                    }
                    _InReceive = false;
                }
            }
            // получаем сообщение об ошибке
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            finally
            {
                //receiver.Close();
            }
        }

        public void Stop()
        {
            receiver.Close();
            receiver = new UdpClient(remotePort);
            receiver.JoinMulticastGroup(GroupIPAdress, localIPAdress);
        }

        // главная функция по принятию сообщений из сети
        public void ReceiveFrame()
        {
            IPEndPoint remoteIp = null;
            string localAddress = LocalIPAddress();
            while (DoLoop)
            {
                try
                {
                    if (receiver is object)
                    {

                        // получаем данные
                        byte[] data = receiver.Receive(ref remoteIp);
                        APackage package = ByteArrayToObject(data) as APackage;

                        if (package.FirstPackage)
                        {
                            byte[] buffer = new byte[package.Length];
                            Array.Copy(package.Data, 0, buffer, 0, package.Data.Length);
                            int dCounter = package.Data.Length;

                            if (package.Length > AServer.PackageLength)
                            {
                                while (dCounter < buffer.Length)
                                {
                                    byte[] temp = receiver.Receive(ref remoteIp);
                                    package = ByteArrayToObject(temp) as APackage;
                                    if (package.FirstPackage) break;
                                    Array.Copy(package.Data, 0, buffer, dCounter, package.Data.Length);
                                    dCounter += package.Data.Length;
                                    //Console.WriteLine("RECEIVE: " + package.Data.Length + " (" + dCounter + " / " + buffer.Length + ")");
                                }
                            }
                            else buffer = package.Data;

                            // вызываем событие о получении сообщения
                            AFrame frame = (AFrame)ByteArrayToObject(buffer);
                            Result = frame;
                            Receive?.Invoke(frame);
                        }
                    }
                }
                // получаем сообщение об ошибке
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }
                finally
                {
                    //receiver.Close();
                }
            }
        }

        // все что ниже - смотри класс AServer - идентично

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
