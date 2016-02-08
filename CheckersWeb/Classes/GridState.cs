using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Classes
{
    public enum GridState
    {
        NULL = -1,
        EMPTY = 0,
        BLACKPAWN = 1,
        WHITEPAWN = 2,
        BLACKKING = 3,
        WHITEKING = 4
    }    
}