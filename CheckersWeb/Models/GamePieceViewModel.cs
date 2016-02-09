using CheckersWeb.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Models
{
    public class GamePieceViewModel
    {
        public string Position { get; set; }

        public GridState Color { get; set; }

        public int Index { get; set; }

        public bool IsPromoted { get; set; }

        public override string ToString()
        {
            return Position + " " + Color;
        }
    }
}