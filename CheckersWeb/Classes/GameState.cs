using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Classes
{
    public enum GameState
    {
        DEFAULTGAME = 0,
        BLACKTURN = 1,
        WHITETURN = 2,
        BLACKWIN = 3,
        WHITEWIN = 4,
        INMOVE = 5
    }
}