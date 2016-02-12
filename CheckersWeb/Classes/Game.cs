using CheckersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CheckersWeb.Classes
{
    public class Game
    {
        private const int BOARDSIZE = 8;
        private const int TILESIZE = 60;
        private const int TOPMARGIN = 35;
        private const int LEFTMARGIN = 35;

        public IEnumerable<GamePieceViewModel> InitBoard()
        {
            var Pieces = new List<GamePieceViewModel>();
            for (int i = 0; i < (BOARDSIZE * BOARDSIZE); i++)
            {
                int iRow = i / BOARDSIZE;
                int iColumn = i % BOARDSIZE;

                if (iRow > 4 && ((iRow % 2 == 0 && iColumn % 2 == 0) || (iRow % 2 == 1 && iColumn % 2 == 1)) == false)
                {
                    Pieces.Add(new GamePieceViewModel { Color = GridState.BLACKPAWN, Position = "sq_" + i, Index = i });
                }
                else if (iRow < 3 && ((iRow % 2 == 0 && iColumn % 2 == 0) || (iRow % 2 == 1 && iColumn % 2 == 1)) == false)
                {
                    Pieces.Add(new GamePieceViewModel { Color = GridState.WHITEPAWN, Position = "sq_" + i, Index = i });
                }
                else
                {
                    if(((iRow % 2 == 0 && iColumn % 2 == 0) || (iRow % 2 == 1 && iColumn % 2 == 1)))
                    {
                        Pieces.Add(new GamePieceViewModel { Color = GridState.NULL, Position = "sq_" + i, Index = i });
                    }
                    else
                    {
                        Pieces.Add(new GamePieceViewModel { Color = GridState.EMPTY, Position = "sq_" + i, Index = i });
                    }
                }                
            }

            return Pieces;
        }
    }
}