using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CheckersWeb.Classes;

namespace CheckersWeb.Models
{
    public class BoardViewModel
    {
        public bool IsLegalMove { get; set; }

        public GameState GameState {get; set;}

        public string Message { get; set; }

        public IEnumerable<GamePieceViewModel> Pieces { get; set; }
    }
}