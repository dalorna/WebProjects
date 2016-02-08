﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Models
{
    public class GamePieceViewModel
    {
        public string Position { get; set; }
        public string Color { get; set; }
        public int Index { get; set; }

        public override string ToString()
        {
            return Position + " " + Color;
        }
    }
}