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
        DRAW = 5
    }

    public enum PraetorianGameState
    {
        DEFAULTGAME = 0,
        ASSASSINTURN = 1,
        ROOKTURN = 2,
        ASSASSINWIN =3,
        ROOKWIN = 4
    }
}