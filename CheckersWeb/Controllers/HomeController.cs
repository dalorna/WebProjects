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

                    if (pos.FirstOrDefault(f => f.Color == colorMovingPiece && f.Index == int.Parse(FromPosition)) == null || string.IsNullOrWhiteSpace(toPosition))
                        return Json(new BoardViewModel { IsLegalMove = false, Pieces = pos });

                    var islegal = IsLegalMove(colorMovingPiece, int.Parse(FromPosition));
                    if ((islegal.IsMove || islegal.IsCapture) == false)
                        return Json(new BoardViewModel { IsLegalMove = false, Pieces = pos });

                    //var move = new GamePieceViewModel { Color = colorMovingPiece, Index = int.Parse(toPosition), Position = "sq_" + toPosition };
                    //pos.FirstOrDefault(f => f.Index == int.Parse(toPosition)).Color = colorMovingPiece;
                    //current.Color = GridState.EMPTY;
                    //Board = pos;

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

        static Predicate<List<int>> ByGridForWhite(GridState entry, int i)
        {
            return delegate (List<int> x)
            {
                int iIndex = x.FindIndex(ByInt(i));
                bool bCanMoveForwardForWhite = iIndex + 1 < x.Count && iIndex >= 0 && x[iIndex] < x[iIndex + 1];
                bool b = iIndex != -1 && ((entry == GridState.WHITEPAWN && bCanMoveForwardForWhite) || entry == GridState.WHITEKING);
                return b;
            };
        }

        static Predicate<int> ByInt(int iNumb)
        {
            return delegate (int y)
            {
                return y == iNumb;
            };
        }

        public List<List<int>> gameLines = new List<List<int>>()
            {   new List<int> (){1, 8, 0, 0, 0, 0, 0, 0 },
                new List<int> (){3, 10, 17, 24, 0, 0, 0, 0 },
                new List<int> (){5, 12, 19, 26, 33, 40, 0, 0 },
                new List<int> (){7, 14, 21, 28, 35, 42, 49, 56 },
                new List<int> (){23, 30, 37, 44, 51, 58, 0, 0 },
                new List<int> (){39, 46, 53, 60, 0, 0, 0, 0 },
                new List<int> (){55, 62, 0, 0, 0, 0, 0, 0 },
                new List<int> (){5, 14, 23, 0, 0, 0, 0, 0} ,
                new List<int> (){3, 12, 21, 30, 39, 0, 0 , 0 },
                new List<int> (){1, 10, 19, 28, 37, 46, 55, 0 },
                new List<int> (){8, 17, 26, 35, 44, 53, 62, 0 },
                new List<int> (){24, 33, 42, 51, 60, 0, 0, 0 },
                new List<int> (){40, 49, 58, 0, 0, 0, 0, 0 } };


        private Move IsLegalMove(GridState Piece, int iStartIndex)
        {
            var BoardArray = Board.ToList().OrderBy(o => o.Index).Select(s => s.Color).ToArray();
            List<List<int>> linesToEvaluate = new List<List<int>>();
            linesToEvaluate = gameLines.FindAll(ByGridForWhite(BoardArray[iStartIndex], iStartIndex));
            List<List<int>> linesEvaluated = new List<List<int>>();
            List<List<int>> extraLines = new List<List<int>>();
            int _moveSquare = 0;
            int _captureSquare = 0;
            GridState _finalSate = Piece;
            bool _IsMove = false;
            bool _IsCapture = false;

            if (BoardArray[iStartIndex] == GridState.WHITEKING)
            {
                foreach (var bline in linesToEvaluate)
                {
                    List<int> nLine = new List<int>();
                    nLine.AddRange(bline);
                    extraLines.Add(nLine);
                }
            }

            linesToEvaluate.AddRange(extraLines);

            foreach (var line in linesToEvaluate)
            {
                bool bKingReverseLine = linesEvaluated.Contains(line);
                linesEvaluated.Add(line);

                int squareCurrentPieceIsOnIndex = line.IndexOf(iStartIndex);
                int squareToMoveToIndex = BoardArray[iStartIndex] == GridState.WHITEKING ? (bKingReverseLine ? squareCurrentPieceIsOnIndex - 1 : squareCurrentPieceIsOnIndex + 1) : squareCurrentPieceIsOnIndex + 1;
                int captureIndexForWhiteIndex = BoardArray[iStartIndex] == GridState.WHITEKING ? (bKingReverseLine ? squareCurrentPieceIsOnIndex - 2 : squareCurrentPieceIsOnIndex + 2) : squareCurrentPieceIsOnIndex + 2;
                GridState startingSquare = BoardArray[iStartIndex];

                if (squareToMoveToIndex >= 0 && squareToMoveToIndex < line.Count())
                {
                    _moveSquare = line[squareToMoveToIndex];
                    _captureSquare = captureIndexForWhiteIndex > 0 && captureIndexForWhiteIndex < line.Count() ? line[captureIndexForWhiteIndex] : -1;

                    bool bHasCaptureSquareToLandOn = _captureSquare >= 0 && BoardArray[_captureSquare] == GridState.EMPTY;
                    bool bHasPieceThatCanBeCaptued = BoardArray[_moveSquare] == GridState.BLACKKING || BoardArray[_moveSquare] == GridState.BLACKPAWN;
                    bool bHasMoveThatCanBeMade = BoardArray[_moveSquare] == GridState.EMPTY;

                    if ((bHasMoveThatCanBeMade || (bHasCaptureSquareToLandOn && bHasPieceThatCanBeCaptued)) == false)
                        continue;

                    if (BoardArray[_moveSquare] == GridState.EMPTY)
                    {
                        _finalSate = (startingSquare == GridState.WHITEPAWN && (_moveSquare == 56 || _moveSquare == 58 || _moveSquare == 60 || _moveSquare == 62)) ? GridState.WHITEKING : startingSquare;
                        //newValuesForBoardArray[iStartIndex] = GridState.EMPTY;
                        _IsMove = true;
                    }

                    if (_captureSquare > 0 && !_IsMove)
                    {
                        _finalSate = (startingSquare == GridState.WHITEPAWN && (_moveSquare == 56 || _moveSquare == 58 || _moveSquare == 60 || _moveSquare == 62)) ? GridState.WHITEKING : startingSquare;
                        //newValuesForBoardArray[_moveSquare] = GridState.EMPTY;
                        //newValuesForBoardArray[iStartIndex] = GridState.EMPTY;
                        _IsCapture = true;
                    }
                }
            }

            return new Move { StartIndex = iStartIndex, PieceState = _finalSate, IsMove = _IsMove, IsCapture = _IsCapture, CaptureIndex = _captureSquare, MoveIndex = _moveSquare };
        }

        private class Move
        {
            public int CaptureIndex { get; set; }
            public int MoveIndex { get; set; }
            public int StartIndex { get; set; }
            public bool IsMove { get; set; }
            public bool IsCapture { get; set; }
            public GridState PieceState { get; set; }
        }

    }
}