using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Models
{
    public class BoardViewModel
    {
        public bool IsLegalMove {get; set; }

        public IEnumerable<GamePieceViewModel> Pieces { get; set; }
    }
}