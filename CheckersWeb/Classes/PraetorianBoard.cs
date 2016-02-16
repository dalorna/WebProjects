using CheckersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Classes
{
    public class PraetorianBoard
    {
        public int _score = 0;
        //public CellState[] _boardArray;
        public List<PraetorianPieceViewModel> _boardPieces;

        bool _mAssassinTurn;

        public bool GameOver
        {
            get;
            private set;
        }

        public int RecursiveScore
        {
            get;
            private set;
        }

        public static List<List<int>> gameLines = new List<List<int>>()
            {   new List<int> (){0, 1, 2, 3, 4, 5, 6, 7},
                new List<int> (){8, 9, 10, 11, 12, 13, 14, 15 },
                new List<int> (){16, 17, 18, 19, 20, 21, 22, 23 },
                new List<int> (){24, 25, 26, 27, 28, 29, 30, 31 },
                new List<int> (){32, 33, 34, 35, 36, 37, 38, 39 },
                new List<int> (){40, 41, 42, 43, 44, 45, 46, 47 },
                new List<int> (){48, 49, 50, 51, 52, 53, 54, 55 },
                new List<int> (){56, 57, 58, 59, 60, 61, 62, 63 },
                new List<int> (){0, 8, 16, 24, 32, 40, 48, 56},
                new List<int> (){1, 9, 17, 25, 33, 41, 49, 57},
                new List<int> (){2, 10, 18, 26, 34, 46, 50, 58},
                new List<int> (){3, 11, 19, 27, 35, 47, 51, 59},
                new List<int> (){4, 12, 20, 28, 36, 48, 52, 60},
                new List<int> (){5, 13, 21, 29, 37, 49, 53, 61},
                new List<int> (){6, 14, 22, 30, 38, 50, 54, 62},
                new List<int> (){7, 15, 23, 31, 39, 51, 55, 63} };

        public PraetorianBoard(List<PraetorianPieceViewModel> boardPieces, bool AssassinTurn)
        {
            _mAssassinTurn = AssassinTurn;
            _boardPieces = boardPieces;
            ComputerScore();
        }

        public void ComputerScore()
        {
            int ret = 0;
            List<int> squares = new List<int>();
            ret += GetOverallScore(_boardPieces);
            _score = ret;
        }

        private int GetOverallScore(List<PraetorianPieceViewModel> boardPieces)
        {
            int countA = 0;
            int CountP = 0;

            for (int i = 0; i < boardPieces.Count(); i++)
            {
                //Calculate postion of Assassin to targets
                //Calculate Praetorian to question new citizen
                //Calculate # of Citizens Questioned
                //Calculate targets down
                //Calculate Praetorian Position to Assassin
            }

            if (_mAssassinTurn)
            {
                return countA + CountP;
            }

            return -(countA + CountP);
        }

        public int MiniMaxWithDebug(int depth, bool needMax, int alpha, int beta, out PraetorianBoard childWithMax)
        {
            childWithMax = null;
            System.Diagnostics.Debug.Assert(_mAssassinTurn == needMax);
            if (depth == 0 || IsTerminalNode())
            {
                RecursiveScore = _score;
                return _score;
            }

            var childBoards = GetChildren();
            foreach (PraetorianBoard cur in childBoards)
            {
                PraetorianBoard dummy;
                int score = cur.MiniMaxWithDebug(depth - 1, !needMax, alpha, beta, out dummy);
                if (!needMax)
                {
                    if (beta > score)
                    {
                        beta = score;
                        childWithMax = cur;
                        if (alpha >= beta)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    if (alpha < score)
                    {
                        alpha = score;
                        childWithMax = cur;
                        if (alpha >= beta)
                        {
                            break;
                        }
                    }
                }
            }

            RecursiveScore = needMax ? alpha : beta;
            return RecursiveScore;
        }

        public bool IsTerminalNode()
        {
            if (GameOver)
                return true;

            bool bMoveLeftForAssassin = false;
            bool bMoveLeftForPraetorian = false;


            //Very important to determine 
            //If the assassin has been found
            //or if the targets are dead
            for (int i = 0; i < _boardPieces.Count(); i++)
            {
                var piece = _boardPieces[i];
                if (piece.Piece == CellState.PRAETORIAN || piece.Piece == CellState.EMPTY)
                    continue;

                if ((piece.Piece == CellState.PRAETORIAN || piece.Piece == CellState.PRAETORIAN) && bMoveLeftForAssassin == false)
                {
                    bMoveLeftForAssassin = MoveForAssassin(piece, i).Count() > 0; 
                }

                if ((piece.Piece == CellState.PRAETORIAN || piece.Piece == CellState.PRAETORIAN) && bMoveLeftForPraetorian == false)
                {
                    bMoveLeftForPraetorian = MoveForPraetorian(piece, i).Count() > 0; 
                }
            }

            return (_mAssassinTurn && !bMoveLeftForAssassin) || (!_mAssassinTurn && !bMoveLeftForPraetorian);
        }

        public List<PraetorianBoard> MoveForAssassin(PraetorianPieceViewModel piece, int i)
        {
            List<PraetorianBoard> PossibleBoards = new List<PraetorianBoard>();

            if (_mAssassinTurn && ((piece.Piece == CellState.PRAETORIAN || piece.Piece == CellState.EMPTY) == false))
            {
                List<int> possibleMoves = new List<int>() { piece.Index - 9, piece.Index - 8, piece.Index - 7, piece.Index - 1, piece.Index + 1, piece.Index + 7, piece.Index + 8, piece.Index + 9 };

                foreach(int iPosMove in possibleMoves)
                {
                    PraetorianPieceViewModel posPiece = _boardPieces.FirstOrDefault(f => f.Index == iPosMove);
                    if ((posPiece != null && posPiece.Piece == CellState.EMPTY) == false)
                        continue;
                    var newBoard = _boardPieces.Clone().ToList();

                    var to = newBoard[iPosMove];
                    newBoard[iPosMove] = newBoard[i];
                    newBoard[i] = to;

                    PossibleBoards.Add(new PraetorianBoard(newBoard, !_mAssassinTurn));
                }
            }

            return PossibleBoards;
        }

        public List<PraetorianBoard> MoveForPraetorian(PraetorianPieceViewModel piece, int i)
        {
            List<PraetorianBoard> PossibleBoards = new List<PraetorianBoard>();
            if (!_mAssassinTurn && piece.Piece == CellState.EMPTY == false)
            {
                if (piece.Piece != CellState.PRAETORIAN)
                {
                    List<int> possibleMoves = new List<int>() { piece.Index - 9, piece.Index - 8, piece.Index - 7, piece.Index - 1, piece.Index + 1, piece.Index + 7, piece.Index + 8, piece.Index + 9 };

                    foreach (int iPosMove in possibleMoves)
                    {
                        PraetorianPieceViewModel posPiece = _boardPieces.FirstOrDefault(f => f.Index == iPosMove);
                        if ((posPiece != null && posPiece.Piece == CellState.EMPTY) == false)
                            continue;

                        var newBoard = _boardPieces.Clone().ToList();
                    
                        var to = newBoard[iPosMove];
                        newBoard[iPosMove] = newBoard[i];
                        newBoard[i] = to;

                        PossibleBoards.Add(new PraetorianBoard(newBoard, !_mAssassinTurn));
                    }
                }
                else
                {
                    List<List<int>> linesToEvaluate = new List<List<int>>();
                    linesToEvaluate = gameLines.FindAll(ByGridForPraetorian(_boardPieces[i].Piece, i));

                    for (int f = 0; f < linesToEvaluate.Count; f++)
                    {
                        int iStart = linesToEvaluate[f].Find(ByInt(i));
                        for(int index = iStart + 1; index < linesToEvaluate[f].Count; index++)
                        {
                            int iSpace = linesToEvaluate[f][index];
                            if (_boardPieces[iSpace].Piece == CellState.EMPTY)
                            {
                                var newBoard = _boardPieces.Clone().ToList();
                                var to = newBoard[iSpace];
                                newBoard[iSpace] = newBoard[i];
                                newBoard[i] = to;
                                PossibleBoards.Add(new PraetorianBoard(newBoard, !_mAssassinTurn));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    for (int b = linesToEvaluate.Count; b >= 0; b--)
                    {
                        int iStart = linesToEvaluate[b].Find(ByInt(i));
                        for (int index = iStart  - 1; index >= 0; index--)
                        {
                            int iSpace = linesToEvaluate[b][index];
                            if (_boardPieces[iSpace].Piece == CellState.EMPTY)
                            {
                                var newBoard = _boardPieces.Clone().ToList();
                                var to = newBoard[iSpace];
                                newBoard[iSpace] = newBoard[i];
                                newBoard[i] = to;
                                PossibleBoards.Add(new PraetorianBoard(newBoard, !_mAssassinTurn));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return PossibleBoards;
        }

        static Predicate<List<int>> ByGridForPraetorian(CellState entry, int i)
        {
            return delegate (List<int> x)
            {
                return x.FindIndex(ByInt(i)) > 0;
            };
        }

        static Predicate<int> ByInt(int iNumb)
        {
            return delegate (int y)
            {
                return y == iNumb;
            };
        }

        public List<PraetorianBoard> GetChildren()
        {
            List<PraetorianBoard> PossibleBoards = new List<PraetorianBoard>();
            for (int i = 0; i < _boardPieces.Count(); i++)
            {
                if (_boardPieces[i].Piece != CellState.PRAETORIAN && _boardPieces[i].Piece != CellState.EMPTY)
                {
                    PossibleBoards.AddRange(MoveForAssassin(_boardPieces[i], i));

                    PossibleBoards.AddRange(MoveForPraetorian(_boardPieces[i], i));
                }
            }

            return PossibleBoards;
        }

        public PraetorianBoard FindNextMove(int depth)
        {
            PraetorianBoard ret1 = null;
            MiniMaxWithDebug(depth, _mAssassinTurn, int.MinValue + 1, int.MaxValue - 1, out ret1);
            return ret1;
        }
    }

    public class PraetorianGameSetup
    {
        public PraetorianBoard Current { get; private set; }
        PraetorianBoard init;

        public PraetorianGameSetup(List<PraetorianPieceViewModel> boardPieces)
        {
            List<PraetorianPieceViewModel> boardPiecesvalues = boardPieces;
            init = new PraetorianBoard(boardPiecesvalues, true);
            Current = init;
        }

        public PraetorianBoard ComputerMakeMove(int depth)
        {
            PraetorianBoard next = Current.FindNextMove(depth);
            if (next != null)
                Current = next;

            return Current;
        }
    }

    public static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}