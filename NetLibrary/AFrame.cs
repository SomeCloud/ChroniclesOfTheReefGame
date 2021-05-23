using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace NetLibrary
{
    [Serializable]
    public class AFrame
    {

        // тип посылаемого фрейма
        public AMessageType MessageType { get; }

        // идентификатор комнаты
        public int Id { get; }

        // данные посылаемые во фрейме
        public object Data { get; }

        // Адрес источника
        public string SourceAdress { get; }

        // Адрес получателя
        public string DestinationAdress { get; }

        public AFrame(int id, object data, AMessageType messageType, string sourceAdress, string destinationAdress)
        {
            Id = id;
            Data = data;
            MessageType = messageType;
            SourceAdress = sourceAdress;
            DestinationAdress = destinationAdress;
        }

    }
}
