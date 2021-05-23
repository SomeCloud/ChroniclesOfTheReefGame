using System;
using System.Collections.Generic;
using System.Text;

namespace NetLibrary
{
    [Serializable]
    public enum AGameStatus
    {
        // ожидаем игроков
        Wait,
        // игра идет
        Game,
        // игра окончена
        Over
    }
}
