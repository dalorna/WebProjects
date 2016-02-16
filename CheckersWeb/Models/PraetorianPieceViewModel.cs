using CheckersWeb.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Models
{
    public class PraetorianPieceViewModel : AbstractCloneable
    {
        public string Position { get; set; }

        public CellState Piece { get; set; }

        public int Index { get; set; }

        public bool IsTarget { get; set; }

        public bool IsAssassin { get; set; }

        public bool HasBeenQuestions { get; set; }

        public override string ToString()
        {
            return Position + " " + Piece + " " + (IsAssassin ? "Assassin" : (IsTarget ? "Target" : "Pedestrian"));
        }
    }

    public abstract class AbstractCloneable : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}