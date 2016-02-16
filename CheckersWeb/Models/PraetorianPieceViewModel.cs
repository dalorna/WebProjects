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

        public bool HasBeenQuestioned { get; set; }

        public bool IsDead { get; set; }

        public bool IsCaught { get; set; }

        public List<int> MovesMade { get; set; }

        public override string ToString()
        {
            string desc = (IsAssassin ? "Assassin" : (IsTarget ? "Target" : "Pedestrian"));
            if (Piece == CellState.EMPTY || Piece == CellState.PRAETORIAN)
                desc = string.Empty;

            return Position + " " + Piece + " " + desc;
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