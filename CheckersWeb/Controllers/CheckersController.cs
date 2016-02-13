using CheckersWeb.Classes;
using CheckersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckersWeb.Controllers
{
    public class CheckersController : Controller
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
                    Message = "White Turn",
                    GameState = GameState.WHITETURN
                };

                jResult = Json(startModel);
            }
            else if(gameState == (GameState)Enum.Parse(typeof(GameState), "BLACKTURN"))
            {
                var gState = Board.ToList().OrderBy(o => o.Index).Select(s => s.Color).ToArray();
                CheckGame cGame = new CheckGame(gState);
                var aIBoard = cGame.ComputerMakeMove(5);
                Board = aIBoard.BoardArray.Select((s, i) => new GamePieceViewModel { Color = s, Index = i, Position = "sq_" + i });
                var currentState = CheckersWeb.Classes.Board.WhiteHasMove(Board.Select(s => s.Color).ToArray()) ? GameState.WHITETURN : GameState.BLACKWIN;
                var model = new BoardViewModel
                {
                    Pieces = Board,
                    IsLegalMove = true,
                    Message = "White Turn",
                    GameState = currentState
                };
                jResult = Json(model);
            }
            else if (gameState == (GameState)Enum.Parse(typeof(GameState), "WHITETURN"))
            {
                try
                {
                    if (colorMovingPiece.ToString().Contains("WHITE") == false) return Json(new BoardViewModel { IsLegalMove = false, Pieces = Board });
                    PlayerMoves pMoves = new PlayerMoves(Board);

                    var pos = Board.ToList();

                    if (pos.FirstOrDefault(f => f.Color == colorMovingPiece && f.Index == int.Parse(FromPosition)) == null || string.IsNullOrWhiteSpace(toPosition))
                        return Json(new BoardViewModel { IsLegalMove = false, Pieces = pos });

                    var islegal = pMoves.MoveISLegal(colorMovingPiece, int.Parse(FromPosition), int.Parse(toPosition));
                    if ((islegal.IsMove || islegal.IsCapture) == false)
                        return Json(new BoardViewModel { IsLegalMove = false, Pieces = pos });

                    pos.FirstOrDefault(f => f.Index == islegal.StartIndex).Color = GridState.EMPTY;
                    if(islegal.IsMove)
                    {
                        pos.FirstOrDefault(f => f.Index == islegal.MoveIndex).Color = islegal.PieceState;
                    }
                    else
                    {
                        pos.FirstOrDefault(f => f.Index == islegal.MoveIndex).Color = GridState.EMPTY;
                        pos.FirstOrDefault(f => f.Index == islegal.CaptureIndex).Color = islegal.PieceState;
                    }
                    Board = pos;
                    var currentState = CheckersWeb.Classes.Board.BlackHasMove(Board.Select(s => s.Color).ToArray()) ? GameState.BLACKTURN : GameState.WHITEWIN;
                    var model = new BoardViewModel
                    {
                        Pieces = pos,
                        IsLegalMove = true,
                        Message = "Black Turn",
                        GameState = currentState
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