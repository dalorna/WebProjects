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

        public IEnumerable<PraetorianPieceViewModel> InitBoard()
        {
            var Pieces = new List<PraetorianPieceViewModel>();

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
                    Pieces.Add(new PraetorianPieceViewModel { Color = CellState.ROOK, Position = "sq_" + i, Index = i });
                }
                else if (ranArray[i] != 0)
                {
                    Pieces.Add(new PraetorianPieceViewModel { Color = (CellState)Enum.Parse(typeof(CellState), ranArray[i].ToString()), Position = "sq_" + i, Index = i });
                }
                else
                {

                        Pieces.Add(new PraetorianPieceViewModel { Color = CellState.EMPTY, Position = "sq_" + i, Index = i });
                }
            }

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
    }
}