using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Classes
{
    public class PraetorianBoard
    {
        public int _score = 0;
        public CellState[] _boardArray;
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

        public PraetorianBoard(CellState[] valuesForBoardArray, bool AssassinTurn)
        {
            _mAssassinTurn = AssassinTurn;
            _boardArray = valuesForBoardArray;
            ComputerScore();
        }

        public void ComputerScore()
        {
            int ret = 0;
            List<int> squares = new List<int>();
            ret += GetOverallScore(_boardArray);
            _score = ret;
        }

        private int GetOverallScore(CellState[] bArray)
        {
            int countA = 0;
            int CountP = 0;

            for (int i = 0; i < bArray.Count(); i++)
            {
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

            bool bMoveLeftForBlack = false;
            bool bMoveLeftForWhite = false;


            //Very important to determine 
            //If the assassin has been found
            //or if the targets are dead
            for (int i = 0; i < _boardArray.Count(); i++)
            {
                var piece = _boardArray[i];
                if (piece == CellState.PRAETORIAN || piece == CellState.EMPTY)
                    continue;

                if ((piece == CellState.PRAETORIAN || piece == CellState.PRAETORIAN) && bMoveLeftForBlack == false)
                {
                    bMoveLeftForBlack = MoveForAssassin(piece, i).Count() > 0; 
                }

                if ((piece == CellState.PRAETORIAN || piece == CellState.PRAETORIAN) && bMoveLeftForWhite == false)
                {
                    bMoveLeftForWhite = MoveForPraetorian(piece, i).Count() > 0; 
                }
            }

            return (_mAssassinTurn && !bMoveLeftForBlack) || (!_mAssassinTurn && !bMoveLeftForWhite);
        }

        public List<PraetorianBoard> MoveForAssassin(CellState piece, int i)
        {
            return new List<PraetorianBoard>();
        }

        public List<PraetorianBoard> MoveForPraetorian(CellState piece, int i)
        {
            return new List<PraetorianBoard>();
        }

        public List<PraetorianBoard> GetChildren()
        {
            List<PraetorianBoard> PossibleBoards = new List<PraetorianBoard>();
            for (int i = 0; i < _boardArray.Length; i++)
            {
                if (_boardArray[i] != CellState.PRAETORIAN && _boardArray[i] != CellState.EMPTY)
                {
                    PossibleBoards.AddRange(MoveForAssassin(_boardArray[i], i));

                    PossibleBoards.AddRange(MoveForPraetorian(_boardArray[i], i));
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

        public PraetorianGameSetup(CellState[] testBoard)
        {
            CellState[] values = testBoard;
            init = new PraetorianBoard(values, true);
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
}