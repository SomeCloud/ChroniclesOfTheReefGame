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

        public RPlayer(string name)
        {
            Name = name;
        }

    }
}
