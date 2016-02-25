using CheckersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Classes
{
    public class PraetorianGame
    {
        private const int BOARDSIZE = 8;
        private int[] ranArray = new int[64];
        private List<int> ranList = new List<int>();
        public List<PraetorianPieceViewModel> Pieces = new List<PraetorianPieceViewModel>();

        public IEnumerable<PraetorianPieceViewModel> InitBoard()
        {
            for(int j = 1; j < 25; j++)
            {
                GetRandomNumber(j);
            }

            ranList = ranArray.ToList().Where(w => w != 0).ToList();


            for (int i = 0; i < (BOARDSIZE * BOARDSIZE); i++)
            {
                int iRow = i / BOARDSIZE;
                int iColumn = i % BOARDSIZE;

                if ((iRow == 0 && iColumn == 0) || (iRow == 7 && iColumn == 7))
                {
                    Pieces.Add(new PraetorianPieceViewModel { Piece = CellState.PRAETORIAN, Position = "sq_" + i, Index = i, MovesMade = new List<int>() { i } });
                }
                //else if (ranArray[i] != 0)
                //{
                //    Pieces.Add(new PraetorianPieceViewModel { Piece = (CellState)Enum.Parse(typeof(CellState), ranArray[i].ToString()), Position = "sq_" + i, Index = i, MovesMade = new List<int>() { i } });
                //}
                else
                {
                    Pieces.Add(new PraetorianPieceViewModel { Piece = CellState.EMPTY, Position = "sq_" + i, Index = i });
                }
            }

            for (int i = 0; i < BOARDSIZE * 3; i++)
            {
                setPiece(i / 3, 7, i > 0 ? 0 : 1, Pieces);
            }

            //int targetCount = Pieces.Where(w => w.IsTarget).Count();
            //var possT = Pieces.Where(w => w.Piece == CellState.EMPTY).ToList();
            SetTargets(Pieces);
            SetNextTargets(Pieces);
            SetAssassin(Pieces);


            return Pieces;
        }

        private void setPiece(int iRow, int iCol, int iStart, List<PraetorianPieceViewModel> pieces)
        {
            int iC = 0;
            Random ran = new Random();
            var peds = pieces.Where(w => w.Piece == CellState.EMPTY).Select(s => (int)s.Piece).ToList();


            int t = 0;
            int iIndex = 0;

            if (iRow == 7 && iCol == 7)
                iCol = 6;
            
            while (true)
            {
                System.Threading.Thread.Sleep(1);
                iC = ran.Next(iStart, iCol);

                iIndex = iRow * 8 + iC;
                if (pieces[iIndex].Piece == CellState.EMPTY)
                    break;
                
            }
            while (true)
            {
                t = ran.Next(0, ranList.Count() - 1);
                int iPedIndex = 0;

                if(pieces.Where(w => (int)w.Piece == ranList[t]).Count() == 0)
                {
                    iPedIndex = ranList[t];
                    ranList.RemoveAt(t);
                    pieces[iIndex] = new PraetorianPieceViewModel() { Piece = (CellState)Enum.Parse(typeof(CellState), iPedIndex.ToString()), Position = "sq_" + iIndex, Index = iIndex, MovesMade = new List<int>() { iIndex } };
                    break;
                }
            }
        }

        private void GetRandomNumber(int cellNum)
        {
            Random ran = new Random();
            int posBoard = ran.Next(1, 62);

            while(true)
            {
                if (ranArray[posBoard] == 0)
                {
                    ranArray[posBoard] = cellNum;
                    break;
                }
                posBoard = ran.Next(1, 62);
            }
        }

        private void SetTargets(List<PraetorianPieceViewModel> possibleTargets)
        {
            Random ran = new Random();
            int t = ran.Next(1, 24);

            var piece = possibleTargets.First(f => (int)f.Piece == t);


            possibleTargets[piece.Index].IsTarget = true;
            //= new PraetorianPieceViewModel { Piece = (CellState)Enum.Parse(typeof(CellState), t.ToString()), Position = "sq_" + i, Index = i, MovesMade = new List<int>() { i }, IsTarget = true };
        }

        private void SetNextTargets(List<PraetorianPieceViewModel> possibleTargets)
        {
            Random ran = new Random();
            int i = 0;
            int otheri = possibleTargets.First(f => f.IsTarget).Index;
            while((i = ran.Next(1, possibleTargets.Count - 1)) == otheri);
            int t = 0;
            while ((t = ran.Next(1, 24)) == (int)possibleTargets.First(f => f.IsTarget).Piece);

            var piece = possibleTargets.First(f => (int)f.Piece == t);
            possibleTargets[piece.Index].IsTarget = true;
            //possibleTargets[i] = new PraetorianPieceViewModel { Piece = (CellState)Enum.Parse(typeof(CellState), t.ToString()), Position = "sq_" + i, Index = i, MovesMade = new List<int>() { i }, IsTarget = true };
        }

        private void SetAssassin(List<PraetorianPieceViewModel> possibleAssassins)
        {
            Random ran = new Random();
            int i = 0;
            int t = 0;
            List<int> notAssassinList = new List<int>() { 1, 8, 9, 54, 55, 62 };
            var targetIndexs = possibleAssassins.Where(w => w.IsTarget).Select(s => s.Index).ToList();
            notAssassinList.AddRange(targetIndexs);

            var targets = possibleAssassins.Where(w => w.IsTarget).Select(s => (int)s.Piece).ToList();
;
            while ((notAssassinList.Contains(i = ran.Next(1, 62))));
            while ((targets.Contains(t = ran.Next(1, 24))));

            var piece = possibleAssassins.First(f => (int)f.Piece == t);
            possibleAssassins[piece.Index].IsAssassin = true;

            //possibleAssassins[i] = new PraetorianPieceViewModel { Piece = (CellState)Enum.Parse(typeof(CellState), t.ToString()), Position = "sq_" + i, Index = i, MovesMade = new List<int>() { i }, IsAssassin = true };
        }
    }
}