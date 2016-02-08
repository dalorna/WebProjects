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
        public JsonResult Index(GridState colorMovingPiece, string FromPosition, string toPosition, GameState gameState)
        {
            var jResult = new JsonResult();
            var game = new Game();
            if (gameState == (GameState)Enum.Parse(typeof(GameState), "DEFAULTGAME"))
            {
                Board = new Game().InitBoard();
                var gState = Board.ToList().OrderBy(o => o.Index).Select(s => s.Color).ToArray();
                CheckGame cGame = new CheckGame(gState);
                var aIBoard = cGame.ComputerMakeMove(5);
                Board = aIBoard.BoardArray.Select((s, i) => new GamePieceViewModel { Color = s, Index = i, Position = "sq_" + i });
                var startModel = new BoardViewModel
                {
                    Pieces = Board,
                    IsLegalMove = true,
                    Message = "White Turn"
                };
                jResult = Json(startModel);
            }
            else if(gameState == (GameState)Enum.Parse(typeof(GameState), "BLACKTURN"))
            {
                var gState = Board.ToList().OrderBy(o => o.Index).Select(s => s.Color).ToArray();
                CheckGame cGame = new CheckGame(gState);
                var aIBoard = cGame.ComputerMakeMove(5);
                Board = aIBoard.BoardArray.Select((s, i) => new GamePieceViewModel { Color = s, Index = i, Position = "sq_" + i });
                var model = new BoardViewModel
                {
                    Pieces = Board,
                    IsLegalMove = true,
                    Message = "White Turn"
                };
                jResult = Json(model);
            }
            else if (gameState == (GameState)Enum.Parse(typeof(GameState), "WHITETURN"))
            {
                try
                {
                    var pos = Board.ToList();
                    var current = pos.FirstOrDefault(f => f.Color == colorMovingPiece && f.Index == int.Parse(FromPosition));

                    if (current == null || string.IsNullOrWhiteSpace(toPosition))
                        return Json(new BoardViewModel { IsLegalMove = false, Pieces = pos });

                    var move = new GamePieceViewModel { Color = colorMovingPiece, Index = int.Parse(toPosition), Position = "sq_" + toPosition };
                    pos.FirstOrDefault(f => f.Index == int.Parse(toPosition)).Color = colorMovingPiece;
                    current.Color = GridState.EMPTY;
                    Board = pos;
                    var model = new BoardViewModel
                    {
                        Pieces = pos,
                        IsLegalMove = true,
                        Message = "Black Turn"
                    };
                    jResult = Json(model);
                }
                catch (Exception ex)
                {
                    return Json(new BoardViewModel { IsLegalMove = false, Pieces = Board, Message = ex.Message });
                }
            }

            return jResult;
        }
    }
}