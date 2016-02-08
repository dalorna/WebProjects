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
        public ActionResult Index(GridState colorMovingPiece, string FromPosition, string toPosition)
        {
            if(colorMovingPiece == (GridState)Enum.Parse(typeof(GridState), "STARTGAME"))
            {
                Board = new Game().InitBoard();
                var gState = Board.ToList().OrderBy(o => o.Index).Select(s => s.Color).ToArray();
                CheckGame cGame = new CheckGame(gState);
                var aIBoard = cGame.ComputerMakeMove(5);
                var pcs = aIBoard.BoardArray.Select((s, i) => new GamePieceViewModel { Color = s, Index = i, Position = "sq_" + i });
                var startModel = new BoardViewModel
                {
                    Pieces = pcs,
                    IsLegalMove = true
                };
                return Json(startModel);
            }

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
                IsLegalMove = true
            };
            return Json(model);
        }
    }
}