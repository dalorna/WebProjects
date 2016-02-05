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
            int iInd = 0;
            int iIndex = 64;
            int iWhite = 1;
            int iBlack = 1;
            var Pieces = new List<GamePieceViewModel>();
            for (int i = 0; i < (BOARDSIZE * BOARDSIZE); i++)
            {
                int iRow = i / 8;
                int iColumn = i % 8;

                //TODO:: Add tiles to board model? This might be better than generating the board in html...???

                if (iRow > 4 && ((iRow % 2 == 0 && iColumn % 2 == 0) || (iRow % 2 == 1 && iColumn % 2 == 1)) == false)
                {
                    Pieces.Add(new GamePieceViewModel { Color = GridState.BLACKPAWN.ToString(), Position = "sq_" + i, Index = i });
                }
                else if (iRow < 3 && ((iRow % 2 == 0 && iColumn % 2 == 0) || (iRow % 2 == 1 && iColumn % 2 == 1)) == false)
                {
                    Pieces.Add(new GamePieceViewModel { Color = GridState.WHITEPAWN.ToString(), Position = "sq_" + i, Index = i });
                }
                else
                {

                }

                    //TilePiece gridCell = new TilePiece(TILESIZE);
                    //gridCell.MouseDown += gameCell_MouseDown;
                    //gridCell.BoardLocation = new BoardLocation { Row = row, Column = col };
                    //gridCell.Location = new Point(LEFTMARGIN + TILESIZE * col, TOPMARGIN + TILESIZE * row);
                    ////mBox.Text += (TOPMARGIN + TILESIZE * row).ToString() + ", " + (5 + TILESIZE * col) + Environment.NewLine;
                    //gridCell.BackColor = (row % 2 == 0 && col % 2 == 0) || (row % 2 == 1 && col % 2 == 1) ? Color.White : Color.Black;
                    //gridCell.DragDrop += gridCell_DragDrop;
                    //gridCell.DragEnter += gridCell_DragEnter;
                    //this.Controls.Add(gridCell);
                    //this.Controls.SetChildIndex(gridCell, ++iInd);

                    //if (row > 4 && gridCell.BackColor == Color.Black)
                    //{
                    //    _GameTurn.BOARDARRAY[row, col] = (int)GridEntry.BLACKPAWN;
                    //    GamePiece piece = new GamePiece() { BoardLocation = new BoardLocation { Row = row, Column = col } };
                    //    piece.MouseDown += piece_MouseDown;
                    //    piece.DragEnter += piece_DragEnter;
                    //    piece.DragDrop += piece_DragDrop;
                    //    piece.PieceState = GridEntry.BLACKPAWN;
                    //    piece.Name = "BLACKPIECE_" + iWhite++;
                    //    piece.Image = GetImage("Checkers.Assets.BlackPiece.png");
                    //    piece.Location = new Point((LEFTMARGIN + TILESIZE * col) + 10, (TOPMARGIN + TILESIZE * row) + 10);

                    //    this.Controls.Add(piece);
                    //    this.Controls.SetChildIndex(piece, ++iIndex);
                    //    piece.BringToFront();
                    //}
                    //else if (row < 3 && gridCell.BackColor == Color.Black)
                    //{
                    //    _GameTurn.BOARDARRAY[row, col] = (int)GridEntry.WHITEPAWN;
                    //    GamePiece piece = new GamePiece() { BoardLocation = new BoardLocation { Row = row, Column = col } };
                    //    piece.MouseDown += piece_MouseDown;
                    //    piece.DragEnter += piece_DragEnter;
                    //    piece.DragDrop += piece_DragDrop;
                    //    piece.PieceState = GridEntry.WHITEPAWN;
                    //    piece.Name = "WHITEPIECE_" + iBlack++;

                    //    piece.Image = GetImage("Checkers.Assets.WhitePiece.png");
                    //    piece.Location = new Point((LEFTMARGIN + TILESIZE * col) + 10, (TOPMARGIN + TILESIZE * row) + 10);

                    //    this.Controls.Add(piece);
                    //    this.Controls.SetChildIndex(piece, ++iIndex);
                    //    piece.BringToFront();
                    //}
                    //else
                    //{
                    //    if (gridCell.BackColor == Color.Black)
                    //    {
                    //        _GameTurn.BOARDARRAY[row, col] = (int)GridEntry.EMPTY;
                    //    }
                    //    else
                    //    {
                    //        _GameTurn.BOARDARRAY[row, col] = (int)GridEntry.NULL;
                    //    }
                    //}
                
            }

            return Pieces;
        }
    }
}