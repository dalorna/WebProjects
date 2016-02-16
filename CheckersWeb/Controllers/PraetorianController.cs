using CheckersWeb.Classes;
using CheckersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckersWeb.Controllers
{
    public class PraetorianController : Controller
    {
        private static PraetorianBoardViewModel _Board = new PraetorianBoardViewModel();

        // GET: Praetorian
        public ActionResult Index()
        {
            var game = new PraetorianGame();
            _Board.Pieces = game.InitBoard().OrderBy(o => o.Index);
            return View(_Board);
        }

        /// <summary>
        /// Index Method to start the game, basically we are seeing what side was choosen,
        /// Assassin always starts first, so if human player is assassin, simply return to let the human move,
        /// Otherwise computer needs to make a move
        /// </summary>
        /// <param name="PlayerSideChoosen"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StartGame(PraetorianGameState playerSideChoosen)
        {
            var jResult = new JsonResult();
            if(playerSideChoosen == PraetorianGameState.ASSASSINTURN)
            {
                //Human needs to make the first move, so set the game state and return out
                _Board.IsAssassinComputer = false;
                _Board.IsLegalMove = true;
                _Board.GameState = PraetorianGameState.ASSASSINTURN;
            }
            else
            {
                _Board.IsAssassinComputer = true;
                PraetorianGameSetup pGame = new PraetorianGameSetup(_Board.Pieces.ToList());
                var newBoard = pGame.ComputerMakeMove(1, true);
                _Board.Pieces = newBoard.BoardPieces;
                _Board.GameState = PraetorianGameState.PRAETORIANTURN;
                _Board.IsLegalMove = true;
            }

            jResult = Json(_Board);
            return jResult;
        }

        /// <summary>
        /// A move for the player
        /// </summary>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PlayerMove(string fromPosition, string toPosition, PraetorianGameState playerSideChoosen)
        {
            var jResult = new JsonResult();

            //Regardless of the playersideChoosen we just need to know that it was a legal move
            if(playerSideChoosen == PraetorianGameState.ASSASSINTURN)
            {
                _Board.GameState = PraetorianGameState.PRAETORIANTURN;
                _Board.IsLegalMove = true;
            }
            else
            {
                _Board.GameState = PraetorianGameState.ASSASSINTURN;
                _Board.IsLegalMove = true;
            }

            jResult = Json(_Board);
            return jResult;
        }

        public JsonResult ComputerMove(PraetorianGameState playerSideChoosen)
        {
            var jResult = new JsonResult();

            if (playerSideChoosen == PraetorianGameState.ASSASSINTURN)
            {
                PraetorianGameSetup pGame = new PraetorianGameSetup(_Board.Pieces.ToList());
                var newBoard = pGame.ComputerMakeMove(2, false);
                _Board.Pieces = newBoard.BoardPieces;
                _Board.GameState = PraetorianGameState.ASSASSINTURN;
                _Board.IsLegalMove = true;
            }
            else
            {
                PraetorianGameSetup pGame = new PraetorianGameSetup(_Board.Pieces.ToList());
                var newBoard = pGame.ComputerMakeMove(1, true);
                _Board.Pieces = newBoard.BoardPieces;
                _Board.GameState = PraetorianGameState.PRAETORIANTURN;
                _Board.IsLegalMove = true;
            }

            jResult = Json(_Board);
            return jResult;
        }

        public JsonResult Interrogate(string positionQuestioned)
        {
            var jResult = new JsonResult();
            jResult = Json(_Board);
            return jResult;
        }
    }
}