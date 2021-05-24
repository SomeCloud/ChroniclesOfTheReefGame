using System;
using System.Collections.Generic;
using System.Text;

namespace NetLibrary
{

    [Serializable]
    public enum AMessageType
    {
        Undefined,
        Connection,
        Disconnection,
        ServerDisconnection,
        RoomInfo,
        StartGame,
        Game,
        Turn,
        GameOver,
        Confirm,
        Request,
        Response,
        Renouncement
    }

}
