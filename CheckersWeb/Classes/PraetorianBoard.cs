﻿using CheckersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Classes
{
    public class PraetorianBoard
    {
        public int _score = 0;
        private List<PraetorianPieceViewModel> _boardPieces;
        public List<PraetorianPieceViewModel> BoardPieces
        {
            get { return _boardPieces; }
        }
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
                new List<int> (){2, 10, 18, 26, 34, 42, 50, 58},
                new List<int> (){3, 11, 19, 27, 35, 43, 51, 59},
                new List<int> (){4, 12, 20, 28, 36, 44, 52, 60},
                new List<int> (){5, 13, 21, 29, 37, 45, 53, 61},
                new List<int> (){6, 14, 22, 30, 38, 46, 54, 62},
                new List<int> (){7, 15, 23, 31, 39, 47, 55, 63} };

        public static List<List<int>> diagonalLines = new List<List<int>>()
            {   new List<int> (){1, 8,-100,-100,-100,-100,-100, -100 },
                new List<int> (){3, 10, 17, 24,-100,-100,-100, -100 },
                new List<int> (){5, 12, 19, 26, 33, 40,-100, -100 },
                new List<int> (){7, 14, 21, 28, 35, 42, 49, 56 },
                new List<int> (){23, 30, 37, 44, 51, 58,-100, -100 },
                new List<int> (){39, 46, 53, 60,-100,-100,-100, -100 },
                new List<int> (){55, 62,-100,-100,-100,-100,-100, -100 },
                new List<int> (){5, 14, 23,-100,-100,-100,-100, -100 } ,
                new List<int> (){3, 12, 21, 30, 39,-100, -100, -100 },
                new List<int> (){1, 10, 19, 28, 37, 46, 55, -100 },
                new List<int> (){8, 17, 26, 35, 44, 53, 62, -100 },
                new List<int> (){24, 33, 42, 51, 60,-100,-100, -100 },
                new List<int> (){40, 49, 58,-100,-100,-100,-100, -100 },
                new List<int>() {2, 9, 16, -100, -100, -100, -100, -100 },
                new List<int>() {4, 11, 18, 25, 32, -100, -100, -100 },
                new List<int>() {6, 13, 20, 27, 34, 41, 48, -100 },
                new List<int>() {15, 22, 29, 36, 43, 50, 57, -100 },
                new List<int>() {31, 38, 45, 52, 59, -100, -100, -100 },
                new List<int>() {47, 54, 61, -100, -100, -100, -100, -100 },
                new List<int>() {48, 57, -100, -100, -100, -100, -100, -100 },
                new List<int>() {32, 41, 50, 59, -100, -100, -100, -100 },
                new List<int>() {16, 25, 34, 43, 52, 61, -100, -100 },
                new List<int>() {0, 9, 18, 27, 36, 45, 54, 63 },
                new List<int>() {2, 11, 20, 29, 38, 47, -100, -100 },
                new List<int>() {4, 13, 22, 31, -100, -100, -100, -100 },
                new List<int>() {6, 15, -100, -100, -100, -100, -100, -100 }
        };

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
                //Calculate how many times a piece has moved and unquestioned for possible assassin...
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

            //Very important to determine 
            //If the assassin has been found
            //or if the targets are dead

            bool capturedAssassin = _boardPieces.Where(w => w.IsAssassin && w.IsCaught).ToList().Count() == 1;
            bool deadTargets = _boardPieces.Where(w => w.IsTarget && w.IsDead).ToList().Count() == 2;

            return capturedAssassin || deadTargets;
        }

        public static void SwapPosition(List<PraetorianPieceViewModel> model, int ito, int ifrom)
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
        }

        private List<PraetorianBoard> MoveNumberPiece(PraetorianPieceViewModel piece, int i)
        {
            List<PraetorianBoard> PossibleBoards = new List<PraetorianBoard>();
            List<int> possibleMoves = new List<int>() { piece.Index - 9, piece.Index - 8, piece.Index - 7, piece.Index - 1, piece.Index + 1, piece.Index + 7, piece.Index + 8, piece.Index + 9 };
            List<List<int>> linesToEvaluate = new List<List<int>>();
            linesToEvaluate.AddRange(gameLines.FindAll(ByGridForPraetorian(i)));
            linesToEvaluate.AddRange(diagonalLines.FindAll(ByGridForPraetorian(i)));

            foreach (int iPosMove in possibleMoves)
            {
                var possibleLine = linesToEvaluate.FindAll(ByGridForPraetorian(iPosMove));
                if (possibleLine.Count == 0) continue; 

                if (possibleLine.Count > 1)
                {
                    throw new Exception("Possible lines count was greater 1");
                }

                if (Math.Abs(possibleLine[0].FindIndex(ByInt(iPosMove)) - possibleLine[0].FindIndex(ByInt(i))) != 1) continue;

                PraetorianPieceViewModel posPiece = _boardPieces.FirstOrDefault(f => f.Index == iPosMove);
                if ((posPiece != null && posPiece.Piece == CellState.EMPTY) == false) continue;

                var newBoard = _boardPieces.Clone().ToList();

                SwapPosition(newBoard, iPosMove, i);
                newBoard = newBoard.OrderBy(o => o.Index).ToList();
                PossibleBoards.Add(new PraetorianBoard(newBoard, !_mAssassinTurn));
            }

            return PossibleBoards;
        }

        public List<PraetorianBoard> MoveForAssassin(PraetorianPieceViewModel piece, int i)
        {
            var PossibleBoards = new List<PraetorianBoard>();

            if (_mAssassinTurn && ((piece.Piece == CellState.PRAETORIAN || piece.Piece == CellState.EMPTY) == false))
            {
                PossibleBoards = MoveNumberPiece(piece, i);
                PossibleBoards.AddRange(GetTargets(piece, i));
            }

            return PossibleBoards;
        }

        public static Predicate<List<int>> findLine(int ifrom, int ito)
        {
            return delegate (List<int> x)
            {
                var bFrom = x.FindIndex(PraetorianBoard.ByInt(ifrom)) >= 0;
                var bTo = x.FindIndex(PraetorianBoard.ByInt(ito)) >= 0;
                return bFrom && bTo;
            };
        }

        public List<PraetorianBoard> GetTargets(PraetorianPieceViewModel assassinPiece, int i)
        {
            List<PraetorianBoard> PossibleBoards = new List<PraetorianBoard>();
            if (assassinPiece.IsAssassin == false)
                return PossibleBoards;

            var allLines = new List<List<int>>();
            allLines.AddRange(gameLines);
            allLines.AddRange(diagonalLines);

            var targets = _boardPieces.Where(w => w.IsTarget);

            foreach (var target in targets)
            {
                var linesToEvaluate = allLines.FindAll(findLine(assassinPiece.Index, target.Index));
                if (linesToEvaluate.Count > 1)
                {
                    throw new Exception("lines to evaluate theortically should only be 1");
                }

                if(linesToEvaluate.Count == 1)
                {
                    var line = linesToEvaluate[0];
                    var targetIndex = line.FindIndex(ByInt(target.Index));//line.FindIndex(ByInt(praetorianPiece.Index));
                    var assassinIndex = line.FindIndex(ByInt(assassinPiece.Index));

                    if ((targetIndex >= 0 && assassinIndex >= 0))
                    {
                        if (Math.Abs(targetIndex - assassinIndex) == 1)
                        {
                            var newBoard = _boardPieces.Clone().ToList();
                            newBoard[target.Index].IsDead = true;
                            newBoard[target.Index].HasBeenQuestioned = false;
                            newBoard[target.Index].Index = target.Index;
                            //newBoard[target.Index].Position = _boardPieces.ToList()[target.Index].Position;
                            newBoard[target.Index].IsAssassin = false;
                            newBoard[target.Index].IsDead = false;
                            newBoard[target.Index].IsTarget = false;
                            newBoard[target.Index].Piece = CellState.EMPTY;
                            newBoard[target.Index].IsCaught = false;
                            newBoard[target.Index].MovesMade = new List<int>();

                            if (newBoard.Where(w => w.IsTarget).Count() == 0)
                            {
                                GameOver = true;
                            }

                            newBoard = newBoard.OrderBy(o => o.Index).ToList();
                            PossibleBoards.Add(new PraetorianBoard(newBoard, !_mAssassinTurn));
                            break;
                        }
                    }
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
                    PossibleBoards = MoveNumberPiece(piece, i);
                }
                else
                {
                    List<List<int>> linesToEvaluate = new List<List<int>>();
                    linesToEvaluate = gameLines.FindAll(ByGridForPraetorian(i));

                    for (int f = 0; f < linesToEvaluate.Count; f++)
                    {
                        int iStart = linesToEvaluate[f].FindIndex(ByInt(i));
                        for (int index = iStart + 1; index < linesToEvaluate[f].Count; index++)
                        {
                            int iSpace = linesToEvaluate[f][index];
                            if (_boardPieces[iSpace].Piece == CellState.EMPTY)
                            {
                                var newBoard = _boardPieces.Clone().ToList();


                                SwapPosition(newBoard, iSpace, i);
                                newBoard = newBoard.OrderBy(o => o.Index).ToList();
                                PossibleBoards.Add(new PraetorianBoard(newBoard, !_mAssassinTurn));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    for (int b = linesToEvaluate.Count - 1; b >= 0; b--)
                    {
                        int iStart = linesToEvaluate[b].FindIndex(ByInt(i));
                        for (int index = iStart - 1; index >= 0; index--)
                        {
                            int iSpace = linesToEvaluate[b][index];
                            if (_boardPieces[iSpace].Piece == CellState.EMPTY)
                            {
                                var newBoard = _boardPieces.Clone().ToList();

                                SwapPosition(newBoard, iSpace, i);
                                newBoard = newBoard.OrderBy(o => o.Index).ToList();
                                PossibleBoards.Add(new PraetorianBoard(newBoard, !_mAssassinTurn));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    PossibleBoards.AddRange(GetSuspects(_boardPieces[i], i));
                }
            }
            return PossibleBoards;
        }

        public List<PraetorianBoard> GetSuspects(PraetorianPieceViewModel praetorianPiece, int i)
        {
            List<PraetorianBoard> PossibleBoards = new List<PraetorianBoard>();
            var allLines = new List<List<int>>();
            allLines.AddRange(gameLines);
            allLines.AddRange(diagonalLines);
            
            foreach (var prox in _boardPieces)
            {
                if (prox.Piece == CellState.EMPTY || prox.Piece == CellState.PRAETORIAN)
                    continue;

                var linesToEvaluate = allLines.FindAll(findLine(praetorianPiece.Index, prox.Index));
                if(linesToEvaluate. Count > 1)
                {
                    throw new Exception("lines to evaluate theortically should only be 1");
                }

                if (linesToEvaluate.Count == 1)
                {
                    var line = linesToEvaluate[0];
                    var firstCop = line.FindIndex(ByInt(praetorianPiece.Index));
                    var suspect = line.FindIndex(ByInt(prox.Index));
                    if ((firstCop >= 0 && suspect >= 0))
                    {
                        if (firstCop >= 0 && Math.Abs(firstCop - suspect) == 1 && _boardPieces[prox.Index].HasBeenQuestioned == false)
                        {
                            var newBoard = _boardPieces.Clone().ToList();
                            newBoard[prox.Index].HasBeenQuestioned = true;
                            if (newBoard[prox.Index].IsAssassin)
                            {
                                newBoard[prox.Index].IsCaught = true;
                                GameOver = true;
                            }

                            newBoard = newBoard.OrderBy(o => o.Index).ToList();
                            PossibleBoards.Add(new PraetorianBoard(newBoard, !_mAssassinTurn));
                            break;
                        }
                    }
                }              
            }

            return PossibleBoards;
        }

        public static Predicate<List<int>> ByGridForPraetorian(int i)
        {
            return delegate (List<int> x)
            {
                var lst = x.FindIndex(ByInt(i)) >= 0;
                return lst;
            };
        }

        public static Predicate<int> ByInt(int iNumb)
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
                if (_boardPieces[i].Piece != CellState.EMPTY)
                {
                    PossibleBoards.AddRange(MoveForAssassin(_boardPieces[i], i));

                    PossibleBoards.AddRange(MoveForPraetorian(_boardPieces[i], i));

                }
            }

            return PossibleBoards;
        }

        public PraetorianBoard FindNextMove(int depth, bool IsAssassinTurn)
        {
            PraetorianBoard ret1 = null;
            _mAssassinTurn = IsAssassinTurn;
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

        public PraetorianBoard ComputerMakeMove(int depth, bool IsAssassinTurn)
        {
            PraetorianBoard next = Current.FindNextMove(depth, IsAssassinTurn);
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