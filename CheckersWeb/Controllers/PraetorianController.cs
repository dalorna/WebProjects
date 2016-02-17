using CheckersWeb.Classes;
using CheckersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckersWeb.Controllers
{
    public class PraetorianController : Controller
    {
        private static PraetorianBoardViewModel _Board = new PraetorianBoardViewModel();
        private const int GAMEDEPTH = 2;

        // GET: Praetorian
        public ActionResult Index()
        {
            var game = new PraetorianGame();
            _Board.Pieces = game.InitBoard().OrderBy(o => o.Index);
            return View(_Board);
        }

        /// <summary>
        /// Index Method to start the game, basically we are seeing what side was choosen,
        /// Assassin always starts first, so if human player is assassin, simply return to let the human move,
        /// Otherwise computer needs to make a move
        /// </summary>
        /// <param name="PlayerSideChoosen"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StartGame(PraetorianGameState playerSideChoosen)
        {
            var jResult = new JsonResult();
            if(playerSideChoosen == PraetorianGameState.ASSASSINTURN)
            {
                //Human needs to make the first move, so set the game state and return out
                _Board.IsAssassinComputer = false;
                _Board.IsLegalMove = true;
                _Board.GameState = PraetorianGameState.ASSASSINTURN;
            }
            else
            {
                _Board.IsAssassinComputer = true;
                PraetorianGameSetup pGame = new PraetorianGameSetup(_Board.Pieces.ToList());
                var newBoard = pGame.ComputerMakeMove(GAMEDEPTH, true);
                _Board.Pieces = newBoard.BoardPieces;
                _Board.GameState = PraetorianGameState.PRAETORIANTURN;
                _Board.IsLegalMove = true;
            }

            jResult = Json(_Board);
            return jResult;
        }

        /// <summary>
        /// A move for the player
        /// </summary>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PlayerMove(string fromPosition, string toPosition, PraetorianGameState playerSideChoosen)
        {
            var jResult = new JsonResult();
            bool bIsLegal = false;

            if (string.IsNullOrEmpty(toPosition))
            {
                _Board.IsLegalMove = bIsLegal;
                return Json(_Board);
            }

            //Regardless of the playersideChoosen we just need to know that it was a legal move
            if(playerSideChoosen == PraetorianGameState.ASSASSINTURN )
            {
                PraetorianPieceViewModel piece = _Board.Pieces.ToList().First(f => f.Position == "sq_" + fromPosition);

                if (piece.Piece != CellState.PRAETORIAN)
                {
                    List<int> possibleMoves = new List<int>() { piece.Index - 9, piece.Index - 8, piece.Index - 7, piece.Index - 1, piece.Index + 1, piece.Index + 7, piece.Index + 8, piece.Index + 9 };
                    List<List<int>> linesToEvaluate = new List<List<int>>();
                    linesToEvaluate.AddRange(PraetorianBoard.gameLines.FindAll(PraetorianBoard.ByGridForPraetorian(piece.Index)));
                    linesToEvaluate.AddRange(PraetorianBoard.diagonalLines.FindAll(PraetorianBoard.ByGridForPraetorian(piece.Index)));

                    int iPossible = possibleMoves.Find(PraetorianBoard.ByInt(int.Parse(toPosition)));
                    int iIndex = possibleMoves.FindIndex(PraetorianBoard.ByInt(int.Parse(toPosition)));

                    var possibleLine = linesToEvaluate.FindAll(PraetorianBoard.ByGridForPraetorian(iPossible));
                    if (possibleLine.Count >= 1 && iIndex >= 0)
                    {
                        if (possibleLine.Count > 1)
                        {
                            throw new Exception("Possible lines count was greater 1");
                        }

                        if (Math.Abs(possibleLine[0].FindIndex(PraetorianBoard.ByInt(iPossible)) - possibleLine[0].FindIndex(PraetorianBoard.ByInt(piece.Index))) == 1)
                        {
                            PraetorianPieceViewModel posPiece = _Board.Pieces.FirstOrDefault(f => f.Index == iPossible);
                            if (posPiece != null && posPiece.Piece == CellState.EMPTY)
                            {
                                var piecesBefore = _Board.Pieces.ToList().Clone();
                                PraetorianBoard.SwapPosition(_Board.Pieces.ToList(), iPossible, piece.Index);
                                _Board.Pieces = _Board.Pieces.OrderBy(o => o.Index);
                                _Board.GameState = PraetorianGameState.PRAETORIANTURN;
                                bIsLegal = true;
                            }
                        }
                    }
                }

                _Board.IsLegalMove = bIsLegal;
            }
            else
            {
                PraetorianPieceViewModel piece = _Board.Pieces.ToList().First(f => f.Position == "sq_" + fromPosition);
                if (piece.Piece != CellState.PRAETORIAN)
                {
                    List<int> possibleMoves = new List<int>() { piece.Index - 9, piece.Index - 8, piece.Index - 7, piece.Index - 1, piece.Index + 1, piece.Index + 7, piece.Index + 8, piece.Index + 9 };
                    List<List<int>> linesToEvaluate = new List<List<int>>();
                    linesToEvaluate.AddRange(PraetorianBoard.gameLines.FindAll(PraetorianBoard.ByGridForPraetorian(piece.Index)));
                    linesToEvaluate.AddRange(PraetorianBoard.diagonalLines.FindAll(PraetorianBoard.ByGridForPraetorian(piece.Index)));

                    int iPossible = possibleMoves.Find(PraetorianBoard.ByInt(int.Parse(toPosition)));
                    int iIndex = possibleMoves.FindIndex(PraetorianBoard.ByInt(int.Parse(toPosition)));

                    var possibleLine = linesToEvaluate.FindAll(PraetorianBoard.ByGridForPraetorian(iPossible));
                    if (possibleLine.Count >= 1 && iIndex >= 0)
                    {
                        if (possibleLine.Count > 1)
                        {
                            throw new Exception("Possible lines count was greater than 1");
                        }

                        if (Math.Abs(possibleLine[0].FindIndex(PraetorianBoard.ByInt(iPossible)) - possibleLine[0].FindIndex(PraetorianBoard.ByInt(piece.Index))) == 1)
                        {
                            PraetorianPieceViewModel posPiece = _Board.Pieces.FirstOrDefault(f => f.Index == iPossible);
                            if (posPiece != null && posPiece.Piece == CellState.EMPTY)
                            {
                                var piecesBefore = _Board.Pieces.ToList().Clone();
                                PraetorianBoard.SwapPosition(_Board.Pieces.ToList(), iPossible, piece.Index);
                                _Board.Pieces = _Board.Pieces.OrderBy(o => o.Index);
                                _Board.GameState = PraetorianGameState.ASSASSINTURN;
                                bIsLegal = true;
                            }
                        }
                    }
                }
                else
                {
                    List<List<int>> linesToEvaluate = new List<List<int>>();
                    linesToEvaluate = PraetorianBoard.gameLines.FindAll(PraetorianBoard.ByGridForPraetorian(piece.Index));

                    var possLine = linesToEvaluate.FindAll(findLine(int.Parse(fromPosition), int.Parse(toPosition)));
                    if(possLine.Count != 1)
                    {
                        if(possLine.Count == 0)
                        {
                            _Board.IsLegalMove = bIsLegal;
                            return Json(_Board);
                        }
                        else
                        {
                            throw new Exception("Possible lines count was greater than 1");
                        }
                    }

                    int iFrom = possLine[0].FindIndex(PraetorianBoard.ByInt(int.Parse(fromPosition)));
                    int iTo = possLine[0].FindIndex(PraetorianBoard.ByInt(int.Parse(toPosition)));
                    int iSpace = 0;
                    if (int.Parse(fromPosition) < int.Parse(toPosition))
                    {
                        for (int index = iFrom + 1; index <= iTo; index++)
                        {
                            iSpace = possLine[0][index];
                            if (_Board.Pieces.ToList()[iSpace].Piece == CellState.EMPTY)
                            {
                                bIsLegal = true;
                            }
                            else
                            {
                                bIsLegal = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int index = iFrom - 1; index >= iTo; index--)
                        {
                            iSpace = possLine[0][index];
                            if (_Board.Pieces.ToList()[iSpace].Piece == CellState.EMPTY)
                            {
                                bIsLegal = true;
                            }
                            else
                            {
                                bIsLegal = false;
                                break;
                            }
                        }
                    }

                    if(bIsLegal)
                    {
                        var newBoard = _Board.Pieces.ToList().Clone().ToList();
                        PraetorianBoard.SwapPosition(_Board.Pieces.ToList(), int.Parse(toPosition), int.Parse(fromPosition));
                        _Board.Pieces = _Board.Pieces.OrderBy(o => o.Index);
                        _Board.GameState = PraetorianGameState.ASSASSINTURN;
                    }
                }

                _Board.IsLegalMove = bIsLegal;
            }

            jResult = Json(_Board);
            return jResult;
        }

        public static Predicate<List<int>> findLine(int ifrom, int ito)
        {
            return delegate(List<int> x)
            {
                var bFrom = x.FindIndex(PraetorianBoard.ByInt(ifrom)) >= 0;
                var bTo = x.FindIndex(PraetorianBoard.ByInt(ito)) >= 0;
                return bFrom && bTo;
            };
        }

        public JsonResult ComputerMove(PraetorianGameState playerSideChoosen)
        {
            var jResult = new JsonResult();

            if (playerSideChoosen == PraetorianGameState.ASSASSINTURN)
            {
                PraetorianGameSetup pGame = new PraetorianGameSetup(_Board.Pieces.ToList());
                var newBoard = pGame.ComputerMakeMove(GAMEDEPTH, false);
                _Board.Pieces = newBoard.BoardPieces.OrderBy(o => o.Index);
                _Board.GameState = PraetorianGameState.ASSASSINTURN;
                _Board.IsLegalMove = true;
            }
            else
            {
                PraetorianGameSetup pGame = new PraetorianGameSetup(_Board.Pieces.ToList());
                var newBoard = pGame.ComputerMakeMove(GAMEDEPTH, true);
                _Board.Pieces = newBoard.BoardPieces.OrderBy(o => o.Index);
                _Board.GameState = PraetorianGameState.PRAETORIANTURN;
                _Board.IsLegalMove = true;
            }

            jResult = Json(_Board);
            return jResult;
        }

        public JsonResult Interrogate(PraetorianPieceViewModel positionQuestioned)
        {
            if (positionQuestioned.IsAssassin)
            {
                _Board.Pieces.First(w => w.Position == positionQuestioned.Position).IsCaught = true;
                _Board.GameState = PraetorianGameState.PRAETORIANWIN;
            }
            else
            {
                _Board.Pieces.First(w => w.Position == positionQuestioned.Position).HasBeenQuestioned = true;
                _Board.GameState = PraetorianGameState.ASSASSINTURN;
            }

            return Json(_Board);
        }

        public JsonResult Assassinate(PraetorianPieceViewModel positionKilled)
        {
            if (positionKilled.IsTarget == false)
                throw new Exception("Not a target... Something is wrong");

            var target = _Board.Pieces.FirstOrDefault(f => f.Index == positionKilled.Index && f.IsTarget);
            if(target != null)
            {
                int indexOfTarget = _Board.Pieces.ToList().FindIndex(findPiece(target));
                var newBoard = _Board.Pieces.ToList();
                
                newBoard[indexOfTarget].HasBeenQuestioned = false;
                newBoard[indexOfTarget].Index = indexOfTarget;
                newBoard[indexOfTarget].Position = _Board.Pieces.ToList()[indexOfTarget].Position;
                newBoard[indexOfTarget].IsAssassin = false;
                newBoard[indexOfTarget].IsDead = false;
                newBoard[indexOfTarget].IsTarget = false;
                newBoard[indexOfTarget].Piece = CellState.EMPTY;
                newBoard[indexOfTarget].IsCaught = false;
                newBoard[indexOfTarget].MovesMade = new List<int>();

                _Board.Pieces = newBoard;
            }

            if (_Board.Pieces.Where(w => w.IsTarget).Count() > 0)
            {
                _Board.GameState = PraetorianGameState.PRAETORIANTURN;
            }
            else
            {
                _Board.GameState = PraetorianGameState.ASSASSINWIN;
            }
            
            return Json(_Board);
        }

        public static Predicate<PraetorianPieceViewModel> findPiece(PraetorianPieceViewModel piece)
        {
            return delegate(PraetorianPieceViewModel find)
            {
                return find.Equals(piece);
            };
        }
    }
}