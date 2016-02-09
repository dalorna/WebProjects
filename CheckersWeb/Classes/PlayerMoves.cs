using CheckersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Classes
{
    public class PlayerMoves
    {
        public IEnumerable<GamePieceViewModel> Board { get; set; }

        public PlayerMoves(IEnumerable<GamePieceViewModel> gameBoard)
        {
            Board = gameBoard;
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

        static Predicate<List<int>> ByGridForWhite(GridState entry, int iStart, int iFinish)
        {
            return delegate(List<int> x)
            {
                int iIndex = x.FindIndex(ByInt(iStart));
                int iFinishIndex = x.FindIndex(ByInt(iFinish));
                bool bCanMoveForwardForWhite = (iIndex + 1 < x.Count && iIndex >= 0 && x[iIndex] < x[iIndex + 1]) && iFinishIndex < x.Count;
                bool b = iIndex != -1 && iFinishIndex != -1 && ((entry == GridState.WHITEPAWN && bCanMoveForwardForWhite) || entry == GridState.WHITEKING);
                return b;
            };
        }

        static Predicate<int> ByInt(int iNumb)
        {
            return delegate(int y)
            {
                return y == iNumb;
            };
        }

        public Move MoveISLegal(GridState Piece, int iStartIndex, int iFinishIndex)
        {
            var BoardArray = Board.ToList().OrderBy(o => o.Index).Select(s => s.Color).ToArray();
            List<List<int>> linesToEvaluate = gameLines.FindAll(ByGridForWhite(BoardArray[iStartIndex], iStartIndex, iFinishIndex));
            List<int> line = linesToEvaluate[0];
            int _moveSquare = 0;
            int _captureSquare = 0;
            GridState _finalSate = Piece;
            bool _IsMove = false;
            bool _IsCapture = false;


            bool bKing = iFinishIndex < iStartIndex && Piece == GridState.WHITEKING;


            int squareCurrentPieceIsOnIndex = line.IndexOf(iStartIndex);
            int squareToMoveToIndex = BoardArray[iStartIndex] == GridState.WHITEKING ? (bKing ? squareCurrentPieceIsOnIndex - 1 : squareCurrentPieceIsOnIndex + 1) : squareCurrentPieceIsOnIndex + 1;
            int captureIndexForWhiteIndex = BoardArray[iStartIndex] == GridState.WHITEKING ? (bKing ? squareCurrentPieceIsOnIndex - 2 : squareCurrentPieceIsOnIndex + 2) : squareCurrentPieceIsOnIndex + 2;
            GridState startingSquare = BoardArray[iStartIndex];

            if (squareToMoveToIndex >= 0 && squareToMoveToIndex < line.Count())
            {
                _moveSquare = line[squareToMoveToIndex];
                _captureSquare = captureIndexForWhiteIndex > 0 && captureIndexForWhiteIndex < line.Count() ? line[captureIndexForWhiteIndex] : -1;

                bool bHasCaptureSquareToLandOn = _captureSquare >= 0 && BoardArray[_captureSquare] == GridState.EMPTY;
                bool bHasPieceThatCanBeCaptued = BoardArray[_moveSquare] == GridState.BLACKKING || BoardArray[_moveSquare] == GridState.BLACKPAWN;
                bool bHasMoveThatCanBeMade = BoardArray[_moveSquare] == GridState.EMPTY;

                if ((bHasMoveThatCanBeMade || (bHasCaptureSquareToLandOn && bHasPieceThatCanBeCaptued)) == false)
                    return new Move { StartIndex = iStartIndex, PieceState = _finalSate, IsMove = false, IsCapture = false, CaptureIndex = _captureSquare, MoveIndex = _moveSquare };

                if (BoardArray[_moveSquare] == GridState.EMPTY)
                {
                    _finalSate = (startingSquare == GridState.WHITEPAWN && (_moveSquare == 56 || _moveSquare == 58 || _moveSquare == 60 || _moveSquare == 62)) ? GridState.WHITEKING : startingSquare;
                    _IsMove = true;
                }

                if (_captureSquare > 0 && !_IsMove)
                {
                    _finalSate = (startingSquare == GridState.WHITEPAWN && (_moveSquare == 56 || _moveSquare == 58 || _moveSquare == 60 || _moveSquare == 62)) ? GridState.WHITEKING : startingSquare;
                    _IsCapture = true;
                }
            }
            

            return new Move { StartIndex = iStartIndex, PieceState = _finalSate, IsMove = _IsMove, IsCapture = _IsCapture, CaptureIndex = _captureSquare, MoveIndex = _moveSquare };
        }


        //public Move MoveISLegal(GridState Piece, int iStartIndex, int iFinishIndex)
        //{
        //    var BoardArray = Board.ToList().OrderBy(o => o.Index).Select(s => s.Color).ToArray();
        //    List<List<int>> linesToEvaluate = new List<List<int>>();
        //    linesToEvaluate = gameLines.FindAll(ByGridForWhite(BoardArray[iStartIndex], iStartIndex, iFinishIndex));
        //    List<List<int>> linesEvaluated = new List<List<int>>();
        //    List<List<int>> extraLines = new List<List<int>>();
        //    int _moveSquare = 0;
        //    int _captureSquare = 0;
        //    GridState _finalSate = Piece;
        //    bool _IsMove = false;
        //    bool _IsCapture = false;

        //    if (BoardArray[iStartIndex] == GridState.WHITEKING)
        //    {
        //        foreach (var bline in linesToEvaluate)
        //        {
        //            List<int> nLine = new List<int>();
        //            nLine.AddRange(bline);
        //            extraLines.Add(nLine);
        //        }
        //    }

        //    linesToEvaluate.AddRange(extraLines);
        //    bool bKing = iFinishIndex < iStartIndex;

        //    foreach (var line in linesToEvaluate)
        //    {
        //        bool bKingReverseLine = linesEvaluated.Contains(line);
        //        linesEvaluated.Add(line);

        //        int squareCurrentPieceIsOnIndex = line.IndexOf(iStartIndex);
        //        int squareToMoveToIndex = BoardArray[iStartIndex] == GridState.WHITEKING ? (bKingReverseLine ? squareCurrentPieceIsOnIndex - 1 : squareCurrentPieceIsOnIndex + 1) : squareCurrentPieceIsOnIndex + 1;
        //        int captureIndexForWhiteIndex = BoardArray[iStartIndex] == GridState.WHITEKING ? (bKingReverseLine ? squareCurrentPieceIsOnIndex - 2 : squareCurrentPieceIsOnIndex + 2) : squareCurrentPieceIsOnIndex + 2;
        //        GridState startingSquare = BoardArray[iStartIndex];

        //        if (squareToMoveToIndex >= 0 && squareToMoveToIndex < line.Count())
        //        {
        //            _moveSquare = line[squareToMoveToIndex];
        //            _captureSquare = captureIndexForWhiteIndex > 0 && captureIndexForWhiteIndex < line.Count() ? line[captureIndexForWhiteIndex] : -1;

        //            bool bHasCaptureSquareToLandOn = _captureSquare >= 0 && BoardArray[_captureSquare] == GridState.EMPTY;
        //            bool bHasPieceThatCanBeCaptued = BoardArray[_moveSquare] == GridState.BLACKKING || BoardArray[_moveSquare] == GridState.BLACKPAWN;
        //            bool bHasMoveThatCanBeMade = BoardArray[_moveSquare] == GridState.EMPTY;

        //            if ((bHasMoveThatCanBeMade || (bHasCaptureSquareToLandOn && bHasPieceThatCanBeCaptued)) == false)
        //                continue;

        //            if (BoardArray[_moveSquare] == GridState.EMPTY)
        //            {
        //                _finalSate = (startingSquare == GridState.WHITEPAWN && (_moveSquare == 56 || _moveSquare == 58 || _moveSquare == 60 || _moveSquare == 62)) ? GridState.WHITEKING : startingSquare;
        //                _IsMove = true;
        //            }

        //            if (_captureSquare > 0 && !_IsMove)
        //            {
        //                _finalSate = (startingSquare == GridState.WHITEPAWN && (_moveSquare == 56 || _moveSquare == 58 || _moveSquare == 60 || _moveSquare == 62)) ? GridState.WHITEKING : startingSquare;
        //                _IsCapture = true;
        //            }
        //        }
        //    }

        //    return new Move { StartIndex = iStartIndex, PieceState = _finalSate, IsMove = _IsMove, IsCapture = _IsCapture, CaptureIndex = _captureSquare, MoveIndex = _moveSquare };
        //}
    }

    public class Move
    {
        public int CaptureIndex { get; set; }
        public int MoveIndex { get; set; }
        public int StartIndex { get; set; }
        public bool IsMove { get; set; }
        public bool IsCapture { get; set; }
        public GridState PieceState { get; set; }
    }
}