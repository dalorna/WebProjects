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
        public List<PraetorianPieceViewModel> Pieces = new List<PraetorianPieceViewModel>();

        public IEnumerable<PraetorianPieceViewModel> InitBoard()
        {
            for(int j = 1; j < 25; j++)
            {
                GetRandomNumber(j);
            }

            for (int i = 0; i < (BOARDSIZE * BOARDSIZE); i++)
            {
                int iRow = i / BOARDSIZE;
                int iColumn = i % BOARDSIZE;

                if ((iRow == 0 && iColumn == 0) || (iRow == 7 && iColumn == 7))
                {
                    Pieces.Add(new PraetorianPieceViewModel { Piece = CellState.PRAETORIAN, Position = "sq_" + i, Index = i, MovesMade = new List<int>() { i } });
                }
                else if (ranArray[i] != 0)
                {
                    Pieces.Add(new PraetorianPieceViewModel { Piece = (CellState)Enum.Parse(typeof(CellState), ranArray[i].ToString()), Position = "sq_" + i, Index = i, MovesMade = new List<int>() { i } });
                }
                else
                {

                        Pieces.Add(new PraetorianPieceViewModel { Piece = CellState.EMPTY, Position = "sq_" + i, Index = i });
                }
            }


            int targetCount = Pieces.Where(w => w.IsTarget).Count();
            var possT = Pieces.Where(w => w.Piece != CellState.PRAETORIAN && w.Piece != CellState.EMPTY).ToList();
            SetTargets(possT);
            SetAssassin(Pieces.Where(w => w.Piece != CellState.PRAETORIAN && w.Piece != CellState.EMPTY && w.IsTarget == false).ToList());

            return Pieces;
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
            int i = ran.Next(0, possibleTargets.Count);
            possibleTargets[i].IsTarget = true;

            if (Pieces.Where(w => w.IsTarget).Count() == 2)
                return;

            SetTargets(possibleTargets.Where(w => w.Piece != CellState.PRAETORIAN && w.Piece != CellState.EMPTY && w.IsTarget == false).ToList());

       
        }

        private void SetAssassin(List<PraetorianPieceViewModel> possibleAssassins)
        {
            Random ran = new Random();
            int i = ran.Next(0, possibleAssassins.Count);
            possibleAssassins[i].IsAssassin = true;
        }
    }
}