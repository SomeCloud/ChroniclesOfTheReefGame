using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace NetLibrary
{
    [Serializable]
    public class RPlayer
    {

        public string Name { get; }

        // локальный хост
        public string IPAdress;

        public RPlayer(string name, string ip)
        {
            Name = name;
            IPAdress = ip;
        }

        public override bool Equals(object obj)
        {
            if (obj is RPlayer player)
                return Name == player.Name && IPAdress == player.IPAdress;
            return false;
        }


    }
}
