using CheckersWeb.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Models
{
    public class PraetorianBoardViewModel
    {
        public bool IsLegalMove { get; set; }

        public PraetorianGameState GameState { get; set; }

        public string Message { get; set; }

        public IEnumerable<PraetorianPieceViewModel> Pieces { get; set; }
    }
}