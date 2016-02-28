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
        private static int _iMoveNumber = 1;
        private static KeyValuePair<int, PraetorianPieceViewModel> _LastMove;
        private static List<KeyValuePair<int, PraetorianPieceViewModel>> _MasterMoveList = new List<KeyValuePair<int, PraetorianPieceViewModel>>();

        // GET: Praetorian
        public ActionResult Index()
        {
            var game = new PraetorianGame();
            _Board.Pieces = game.InitBoard().OrderBy(o => o.Index);
            _LastMove = new KeyValuePair<int, PraetorianPieceViewModel>(0, null);
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
                PraetorianBoard.ComputerState = PraetorianGameState.PRAETORIANTURN;
            }
            else
            {
                _Board.IsAssassinComputer = true;
                PraetorianBoard.ComputerState = PraetorianGameState.ASSASSINTURN;
                PraetorianGameSetup pGame = new PraetorianGameSetup(_Board.Pieces.ToList(), new KeyValuePair<int, PraetorianPieceViewModel>(0, new PraetorianPieceViewModel() { }));
                var newBoard = pGame.ComputerMakeMove(GAMEDEPTH, true);
                _LastMove = new KeyValuePair<int, PraetorianPieceViewModel>(_iMoveNumber++, GetLastMovedPieceFromComputer(newBoard.BoardPieces, _Board.Pieces.ToList()));
                _MasterMoveList.Add(_LastMove);
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
            PraetorianPieceViewModel piece = _Board.Pieces.ToList().First(f => f.Position == "sq_" + fromPosition);

            if (_LastMove.Value != null && piece.Piece == _LastMove.Value.Piece)
            {
                _Board.IsLegalMove = bIsLegal;
                return Json(_Board);
            }

            if (string.IsNullOrEmpty(toPosition))
            {
                _Board.IsLegalMove = bIsLegal;
                return Json(_Board);
            }

            //Regardless of the playersideChoosen we just need to know that it was a legal move
            if (playerSideChoosen == PraetorianGameState.ASSASSINTURN )
            {

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
                                SwapPosition(_Board.Pieces.ToList(), iPossible, piece.Index, _iMoveNumber++);
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
                                SwapPosition(_Board.Pieces.ToList(), iPossible, piece.Index, _iMoveNumber++);
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
                        SwapPosition(_Board.Pieces.ToList(), int.Parse(toPosition), int.Parse(fromPosition), _iMoveNumber++);
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

            PraetorianGameSetup pGame = new PraetorianGameSetup(_Board.Pieces.ToList(), _MasterMoveList[_MasterMoveList.Count - 1]);
            if (playerSideChoosen == PraetorianGameState.ASSASSINTURN)
            {
                var newBoard = pGame.ComputerMakeMove(GAMEDEPTH, false);
                _iMoveNumber++;
                var movePiece = GetLastMovedPieceFromComputer(newBoard.BoardPieces, _Board.Pieces.ToList());
                _LastMove = new KeyValuePair<int, PraetorianPieceViewModel>(_iMoveNumber, movePiece);
                _MasterMoveList.Add(_LastMove);
                _Board.Pieces = newBoard.BoardPieces.OrderBy(o => o.Index);
                _Board.GameState = PraetorianGameState.ASSASSINTURN;
                _Board.IsLegalMove = true;
            }
            else
            {
                var newBoard = pGame.ComputerMakeMove(GAMEDEPTH, true);
                _iMoveNumber++;
                var movePiece = GetLastMovedPieceFromComputer(newBoard.BoardPieces, _Board.Pieces.ToList());
                _LastMove = new KeyValuePair<int, PraetorianPieceViewModel>(_iMoveNumber, movePiece);
                _MasterMoveList.Add(_LastMove);
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
                var qPiece = _Board.Pieces.First(w => w.Position == positionQuestioned.Position);
                qPiece.IsCaught = true;
                _iMoveNumber++;
                _LastMove = new KeyValuePair<int, PraetorianPieceViewModel>(_iMoveNumber, new PraetorianPieceViewModel() { Piece = qPiece.Piece });
                _MasterMoveList.Add(new KeyValuePair<int, PraetorianPieceViewModel>(_iMoveNumber, qPiece));
                _Board.GameState = PraetorianGameState.PRAETORIANWIN;
            }
            else
            {
                var qPiece =_Board.Pieces.First(w => w.Position == positionQuestioned.Position);
                qPiece.HasBeenQuestioned = true;
                _iMoveNumber++;
                _LastMove = new KeyValuePair<int, PraetorianPieceViewModel>(_iMoveNumber, new PraetorianPieceViewModel() { Piece = qPiece.Piece, HasBeenQuestioned = true });
                _MasterMoveList.Add(new KeyValuePair<int, PraetorianPieceViewModel>(_iMoveNumber, qPiece));
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
                newBoard[indexOfTarget].MovesMade = new List<KeyValuePair<int, int>>();

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

        public void SwapPosition(List<PraetorianPieceViewModel> model, int ito, int ifrom, int iMoveNumber)
        {
            var to = model[ito];
            var from = model[ifrom];
            var tempPos = from.Position;
            var tempIndex = from.Index;
            from.Index = to.Index;
            from.Position = to.Position;
            to.Position = tempPos;
            to.Index = tempIndex;
            model[ito] = from;
            model[ifrom] = to;
            model[ito].MovesMade.Add(new KeyValuePair<int, int>(iMoveNumber, ito));
            _LastMove = new KeyValuePair<int, PraetorianPieceViewModel>(iMoveNumber, model[ito]);
            _MasterMoveList.Add(new KeyValuePair<int, PraetorianPieceViewModel>(iMoveNumber, model[ito]));
        }
        
        private PraetorianPieceViewModel GetLastMovedPieceFromComputer(List<PraetorianPieceViewModel> computerMove, List<PraetorianPieceViewModel> previousBoard)
        {
            PraetorianPieceViewModel piece = new PraetorianPieceViewModel();
            foreach(var cPiece in computerMove)
            {
                if (cPiece.Piece != CellState.EMPTY && cPiece.Piece != CellState.PRAETORIAN)
                {
                    var pPiece = previousBoard.First(f => f.Piece == cPiece.Piece);
                    if (pPiece.Index != cPiece.Index)
                    {
                        piece = cPiece;
                        break;
                    }
                }
            }

            return piece;
        }
    }
}