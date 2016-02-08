using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersWeb.Classes
{
    public class AICheckers
    {


    }

    public class CheckGame
    {
        public Board Current { get; private set; }
        Board init;
        private int[] whiteStart = { 1, 3, 5, 7, 8, 10, 12, 14, 17, 19, 21, 23 };
        private int[] blackStart = { 40, 42, 44, 46, 49, 51, 53, 55, 56, 58, 60, 62 };
        private int[] EmptyStart = { 24, 26, 28, 30, 33, 35, 37, 39 };

        public CheckGame()
        {
            GridState[] values = SetUpInitBoard();
            init = new Board(values, true);
            Current = init;
        }

        public CheckGame(GridState[] testBoard)
        {
            GridState[] values = testBoard;
            init = new Board(values, true);
            Current = init;
        }

        private GridState[] SetUpInitBoard()
        {
            var emptyBoard = Enumerable.Repeat(GridState.NULL, 64).ToArray();
            for (int i = 0; i < 64; i++)
            {
                if (whiteStart.Contains(i))
                {
                    emptyBoard[i] = GridState.WHITEPAWN;
                }

                if (blackStart.Contains(i))
                {
                    emptyBoard[i] = GridState.BLACKPAWN;
                }

                if (EmptyStart.Contains(i))
                {
                    emptyBoard[i] = GridState.EMPTY;
                }
            }

            return emptyBoard;
        }

        public Board ComputerMakeMove(int depth)
        {
            Board next = Current.FindNextMove(depth);
            if (next != null)
                Current = next;

            return Current;
        }

        public Board GetInitNode()
        {
            return init;
        }
    }

    public class Board
    {
        public int m_Score = 0;
        public GridState[] BoardArray;
        bool m_TurnForPlayerBlack;
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

        public Board(GridState[] valuesForBoardArray, bool turnForPlayerBlack)
        {
            m_TurnForPlayerBlack = turnForPlayerBlack;
            BoardArray = valuesForBoardArray;
            ComputerScore();
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

        public void ComputerScore()
        {
            int ret = 0;
            List<int> squares = new List<int>();

            //for (int i = 0; i < gameLines.Count; i++)
            //{
            //    squares = new List<int>() { gameLines[i][0],
            //    gameLines[i][1],
            //    gameLines[i][2],
            //    gameLines[i][3],
            //    gameLines[i][4],
            //    gameLines[i][5],
            //    gameLines[i][6],
            //    gameLines[i][7]};
            //    ret += GetScoreForOneLine(new GridState[] {
            //        BoardArray[gameLines[i][0]],
            //        BoardArray[gameLines[i][1]],
            //        BoardArray[gameLines[i][2]],
            //        BoardArray[gameLines[i][3]],
            //        BoardArray[gameLines[i][4]],
            //        BoardArray[gameLines[i][5]],
            //        BoardArray[gameLines[i][6]],
            //        BoardArray[gameLines[i][7]]
            //    }, squares);

            //}

            ret += GetOverallScore(BoardArray);
            m_Score = ret;
        }

        private int GetScoreForOneLine(GridState[] values, List<int> squares)
        {
            int CountB = 0;
            int CountW = 0;
            int CountBK = 0;
            int CountWK = 0;

            //Might need to calculate each move on a line and weight them
            foreach (var v in values)
            {
                if (v == GridState.NULL || v == GridState.EMPTY)
                    continue;

                if (v == GridState.BLACKPAWN)
                {
                    CountB++;
                }

                if (v == GridState.WHITEPAWN)
                {
                    CountW++;
                }

                if (v == GridState.BLACKKING)
                {
                    CountB += 1;
                    CountBK += 3;
                }

                if (v == GridState.WHITEKING)
                {
                    CountW += 1;
                    CountWK += 3;
                }
            }

            int advantage = 3;
            int advOpp = 5;
            if (m_TurnForPlayerBlack)
            {
                int ret1 = ((int)Math.Pow(CountB, 2) + ((int)Math.Pow(CountBK, advantage))) - (CountW * advOpp);
                return ret1;
            }

            int ret2 = ((int)Math.Pow(CountW, 2) + ((int)Math.Pow(CountWK, advantage))) - (CountB * advOpp);
            return ret2;
            //return -((int)Math.Pow(CountW, 3) + (CountWK * advantage)) - CountB;;
        }

        private int GetOverallScore(GridState[] bArray)
        {
            int CountB = 0;
            int CountW = 0;
            int CountBK = 0;
            int CountWK = 0;
            //var bMoveLeftForBlack = false;
            //var bMoveLeftForWhite = false;



            //for (int i = 0; i < BoardArray.Count(); i++)
            //{
            //    var piece = BoardArray[i];
            //    if (piece == GridState.BLACKPAWN || piece == GridState.BLACKKING)
            //    {
            //        bMoveLeftForBlack = moveLeftForBlack(piece, i);
            //        if (bMoveLeftForBlack)
            //            break;
            //    }
            //}

            //for (int i = 0; i < BoardArray.Count(); i++)
            //{
            //    var piece = BoardArray[i];
            //    if (piece == GridState.WHITEKING || piece == GridState.WHITEPAWN)
            //    {
            //        bMoveLeftForWhite = moveLeftForWhite(piece, i);
            //        if (bMoveLeftForWhite)
            //            break;
            //    }
            //}

            //if ((m_TurnForPlayerBlack && !bMoveLeftForBlack) || (!m_TurnForPlayerBlack && !bMoveLeftForWhite))
            //    GameOver = true;

            for (int i = 0; i < bArray.Count(); i++)
            {
                var piece = bArray[i];
                if (piece == GridState.NULL || piece == GridState.EMPTY)
                    continue;

                if (piece == GridState.BLACKPAWN)
                {
                    CountB++;
                }

                if (piece == GridState.WHITEPAWN)
                {
                    CountW++;
                }

                if (piece == GridState.BLACKKING)
                {
                    CountB += 1;
                    CountBK += 3;
                }

                if (piece == GridState.WHITEKING)
                {
                    CountW += 1;
                    CountWK += 3;
                }
            }

            if (m_TurnForPlayerBlack)
            {
                return (CountBK * 3) + ((CountBK - CountWK) * 5) + (CountB - CountW);
            }

            return -((CountWK * 3) + ((CountWK - CountWK) * 5) + (CountW - CountB));
        }

        public int MiniMax(int depth, int alpha, int beta, out Board childWithMax)
        {
            childWithMax = null;
            if (depth == 0 || IsTerminalNode())
            {
                //When it is turn for WhitePlayer, we need to find the minimum score.
                RecursiveScore = m_Score;
                return m_TurnForPlayerBlack ? m_Score : -m_Score;
            }

            foreach (Board currentBoard in GetChildren())
            {
                Board dummy;
                int score = -currentBoard.MiniMax(depth - 1, -beta, -alpha, out dummy);
                if (alpha < score)
                {
                    alpha = score;
                    childWithMax = currentBoard;
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
            }

            RecursiveScore = alpha;
            return alpha;
        }

        public int MiniMaxWithDebug(int depth, bool needMax, int alpha, int beta, out Board childWithMax)
        {
            childWithMax = null;
            System.Diagnostics.Debug.Assert(m_TurnForPlayerBlack == needMax);
            if (depth == 0 || IsTerminalNode())
            {
                RecursiveScore = m_Score;
                return m_Score;
            }

            var childBoards = GetChildren();
            foreach (Board cur in childBoards)
            {
                Board dummy;
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

        public Board FindNextMove(int depth)
        {
            //Board ret = null;
            Board ret1 = null;
            MiniMax(depth, int.MinValue + 1, int.MaxValue - 1, out ret1);
            //MiniMaxWithDebug(depth, m_TurnForPlayerBlack, int.MinValue + 1, int.MaxValue - 1, out ret);

            //compare the two versions of MiniMax give the same results
            //if (!IsSameBoard(ret, ret1, true))
            //{
            //    Console.WriteLine("ret={0}\n,!= ret1={1},\ncur={2}", ret, ret1, this);
            //    throw new Exception("Two MinMax functions don't match.");
            //}
            return ret1;
        }

        static bool IsSameBoard(Board a, Board b, bool compareRecursiveScore)
        {
            if (a == b)
                return true;
            if (a == null || b == null)
                return false;
            for (int i = 0; i < a.BoardArray.Length; i++)
            {
                if (a.BoardArray[i] != b.BoardArray[i])
                    return false;
            }

            if (a.m_Score != b.m_Score)
                return false;

            if (compareRecursiveScore && Math.Abs(a.RecursiveScore) != Math.Abs(b.RecursiveScore))
                return false;

            return true;
        }

        static Predicate<List<int>> ByGridForBlack(GridState entry, int i)
        {
            return delegate (List<int> x)
            {
                int iIndex = x.FindIndex(ByInt(i));
                bool bCanMoveForwardForBlack = iIndex - 1 >= 0 && x[iIndex] > x[iIndex - 1];
                bool b = iIndex != -1 && ((entry == GridState.BLACKPAWN && bCanMoveForwardForBlack) || entry == GridState.BLACKKING);
                return b;
            };
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

        public bool IsTerminalNode()
        {
            if (GameOver)
                return true;

            bool bMoveLeftForBlack = false;
            bool bMoveLeftForWhite = false;


            //Very important to determine 
            //if it's Black's turn and Black has a move or if Black or a piece left
            //Or if it's White turn and White has a move or if White or a piece left 
            //If there are no more moves to make for a player then you have reached a leaf node
            for (int i = 0; i < BoardArray.Count(); i++)
            {
                var piece = BoardArray[i];
                if (piece == GridState.NULL || piece == GridState.EMPTY)
                    continue;

                if ((piece == GridState.BLACKPAWN || piece == GridState.BLACKKING) && bMoveLeftForBlack == false)
                {
                    bMoveLeftForBlack = MoveForBlack(piece, i).Count() > 0; //moveLeftForBlack(piece, i);
                }

                if ((piece == GridState.WHITEKING || piece == GridState.WHITEPAWN) && bMoveLeftForWhite == false)
                {
                    bMoveLeftForWhite = MoveForWhite(piece, i).Count() > 0; //moveLeftForWhite(piece, i);
                }
            }

            return (m_TurnForPlayerBlack && !bMoveLeftForBlack) || (!m_TurnForPlayerBlack && !bMoveLeftForWhite);
        }

        public List<Board> GetChildren()
        {
            List<Board> PossibleBoards = new List<Board>();
            for (int i = 0; i < BoardArray.Length; i++)
            {
                if (BoardArray[i] != GridState.NULL && BoardArray[i] != GridState.EMPTY)
                {
                    PossibleBoards.AddRange(MoveForWhite(BoardArray[i], i));

                    PossibleBoards.AddRange(MoveForBlack(BoardArray[i], i));
                }
            }

            return PossibleBoards;
        }

        public List<Board> MoveForBlack(GridState piece, int i)
        {
            List<Board> PossibleBoards = new List<Board>();

            if (m_TurnForPlayerBlack && (piece == GridState.BLACKKING || piece == GridState.BLACKPAWN))
            {
                List<List<int>> linesToEvaluate = new List<List<int>>();
                linesToEvaluate = gameLines.FindAll(ByGridForBlack(piece, i));
                List<List<int>> linesEvaluated = new List<List<int>>();
                List<List<int>> extraLines = new List<List<int>>();

                if (piece == GridState.BLACKKING)
                {
                    foreach (var bline in linesToEvaluate)
                    {
                        var nLine = new List<int>();
                        nLine.AddRange(bline);
                        extraLines.Add(nLine);
                    }
                }

                linesToEvaluate.AddRange(extraLines);

                foreach (var line in linesToEvaluate)
                {
                    bool bKingReverseLine = linesEvaluated.Any(y => y.All(j => line.Contains(j)));
                    linesEvaluated.Add(line);
                    bool isMove = false;
                    int squareCurrentPieceIsOn = line.IndexOf(i);
                    int squareToMoveTo = piece == GridState.BLACKKING ? (bKingReverseLine ? squareCurrentPieceIsOn + 1 : squareCurrentPieceIsOn - 1) : squareCurrentPieceIsOn - 1;
                    int captureIndexForBlack = piece == GridState.BLACKKING ? (bKingReverseLine ? squareCurrentPieceIsOn + 2 : squareCurrentPieceIsOn - 2) : squareCurrentPieceIsOn - 2;
                    int moveSquare = 0;
                    int captureSquare = 0;
                    GridState staringSquare = piece;
                    GridState[] newValuesForBoardArray = (GridState[])BoardArray.Clone();

                    if (squareToMoveTo >= 0 && squareToMoveTo < line.Count())
                    {
                        moveSquare = line[squareToMoveTo];
                        captureSquare = captureIndexForBlack >= 0 && captureIndexForBlack < 8 ? line[captureIndexForBlack] : -1;

                        bool bHasCaptureSquareToLandOn = captureSquare >= 0 && BoardArray[captureSquare] == GridState.EMPTY;
                        bool bHasPieceThatCanBeCaptued = BoardArray[moveSquare] == GridState.WHITEKING || BoardArray[moveSquare] == GridState.WHITEPAWN;
                        bool bHasMoveThatCanBeMade = BoardArray[moveSquare] == GridState.EMPTY;

                        if ((bHasMoveThatCanBeMade || (bHasCaptureSquareToLandOn && bHasPieceThatCanBeCaptued)) == false)
                            continue;

                        if (BoardArray[moveSquare] == GridState.EMPTY)
                        {
                            newValuesForBoardArray[moveSquare] = (staringSquare == GridState.BLACKPAWN && (moveSquare == 1 || moveSquare == 3 || moveSquare == 5 || moveSquare == 7)) ? GridState.BLACKKING : staringSquare;
                            newValuesForBoardArray[i] = GridState.EMPTY;
                            PossibleBoards.Add(new Board(newValuesForBoardArray, !m_TurnForPlayerBlack));
                            isMove = true;
                        }

                        if (captureSquare > 0 && !isMove)
                        {
                            newValuesForBoardArray[captureSquare] = (staringSquare == GridState.BLACKPAWN && (moveSquare == 1 || moveSquare == 3 || moveSquare == 5 || moveSquare == 7)) ? GridState.BLACKKING : staringSquare;
                            newValuesForBoardArray[moveSquare] = GridState.EMPTY;
                            newValuesForBoardArray[i] = GridState.EMPTY;
                            PossibleBoards.Add(new Board(newValuesForBoardArray, !m_TurnForPlayerBlack));
                        }
                    }
                }
            }
            return PossibleBoards;
        }

        public List<Board> MoveForWhite(GridState piece, int i)
        {
            List<Board> PossibleBoards = new List<Board>();
            if (!m_TurnForPlayerBlack && (BoardArray[i] == GridState.WHITEKING || BoardArray[i] == GridState.WHITEPAWN))
            {
                List<List<int>> linesToEvaluate = new List<List<int>>();
                linesToEvaluate = gameLines.FindAll(ByGridForWhite(BoardArray[i], i));
                List<List<int>> linesEvaluated = new List<List<int>>();
                List<List<int>> extraLines = new List<List<int>>();

                if (BoardArray[i] == GridState.WHITEKING)
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

                    bool isMove = false;

                    int squareCurrentPieceIsOnIndex = line.IndexOf(i);
                    int squareToMoveToIndex = BoardArray[i] == GridState.WHITEKING ? (bKingReverseLine ? squareCurrentPieceIsOnIndex - 1 : squareCurrentPieceIsOnIndex + 1) : squareCurrentPieceIsOnIndex + 1;
                    int captureIndexForWhiteIndex = BoardArray[i] == GridState.WHITEKING ? (bKingReverseLine ? squareCurrentPieceIsOnIndex - 2 : squareCurrentPieceIsOnIndex + 2) : squareCurrentPieceIsOnIndex + 2;
                    int moveSquare = 0;
                    int captureSquare = 0;
                    GridState staringSquare = BoardArray[i];
                    GridState[] newValuesForBoardArray = (GridState[])BoardArray.Clone();

                    if (squareToMoveToIndex >= 0 && squareToMoveToIndex < line.Count())
                    {
                        moveSquare = line[squareToMoveToIndex];
                        captureSquare = captureIndexForWhiteIndex > 0 && captureIndexForWhiteIndex < line.Count() ? line[captureIndexForWhiteIndex] : -1;

                        bool bHasCaptureSquareToLandOn = captureSquare >= 0 && BoardArray[captureSquare] == GridState.EMPTY;
                        bool bHasPieceThatCanBeCaptued = BoardArray[moveSquare] == GridState.BLACKKING || BoardArray[moveSquare] == GridState.BLACKPAWN;
                        bool bHasMoveThatCanBeMade = BoardArray[moveSquare] == GridState.EMPTY;

                        if ((bHasMoveThatCanBeMade || (bHasCaptureSquareToLandOn && bHasPieceThatCanBeCaptued)) == false)
                            continue;

                        if (BoardArray[moveSquare] == GridState.EMPTY)
                        {
                            newValuesForBoardArray[moveSquare] = (staringSquare == GridState.WHITEPAWN && (moveSquare == 56 || moveSquare == 58 || moveSquare == 60 || moveSquare == 62)) ? GridState.WHITEKING : staringSquare;
                            newValuesForBoardArray[i] = GridState.EMPTY;
                            PossibleBoards.Add(new Board(newValuesForBoardArray, !m_TurnForPlayerBlack));
                            isMove = true;
                        }

                        if (captureSquare > 0 && !isMove)
                        {
                            newValuesForBoardArray[captureSquare] = (staringSquare == GridState.WHITEPAWN && (moveSquare == 56 || moveSquare == 58 || moveSquare == 60 || moveSquare == 62)) ? GridState.WHITEKING : staringSquare;
                            newValuesForBoardArray[moveSquare] = GridState.EMPTY;
                            newValuesForBoardArray[i] = GridState.EMPTY;
                            PossibleBoards.Add(new Board(newValuesForBoardArray, !m_TurnForPlayerBlack));
                        }
                    }
                }
            }

            return PossibleBoards;
        }
    }
}
