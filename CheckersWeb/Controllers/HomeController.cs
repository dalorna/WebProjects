using CheckersWeb.Classes;
using CheckersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckersWeb.Controllers
{
    public class HomeController : Controller
    {
        private static IEnumerable<GamePieceViewModel> Board = new List<GamePieceViewModel>();

        public ActionResult Index()
        {
            var game = new Game();
            Board = game.InitBoard();
            var model = new BoardViewModel
            {
                Pieces = Board
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string colorMovingPiece, string FromPosition, string toPosition)
        {
            var game = new Game();
          
            var pos = Board.ToList();
            var current = pos.FirstOrDefault(f => f.Color == colorMovingPiece && f.Index == int.Parse(FromPosition));

            if (current == null || string.IsNullOrWhiteSpace(toPosition))
                return Json(new BoardViewModel { IsLegalMove = false, Pieces = pos });

            var move = new GamePieceViewModel { Color = colorMovingPiece, Index = int.Parse(toPosition), Position = "sq_" + toPosition };
            pos.Remove(current);
            pos.Add(move);
            Board = pos;
            var model = new BoardViewModel
            {
                Pieces = pos,
                IsLegalMove = false
            };
            return Json(model);
        }
    }
}