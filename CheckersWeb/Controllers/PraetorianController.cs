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
        private static IEnumerable<PraetorianPieceViewModel> Board = new List<PraetorianPieceViewModel>();

        // GET: Praetorian
        public ActionResult Index()
        {
            var game = new PraetorianGame();
            Board = game.InitBoard();
            var model = new PraetorianBoardViewModel()
            {
                Pieces = Board
            };
            return View(model);
        }

        /// <summary>
        /// Index Method to start the game, basically we are seeing what side was choosen
        /// </summary>
        /// <param name="PlayerSideChoosen"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StartGame(PraetorianGameState PlayerSideChoosen)
        {
            var jResult = new JsonResult();
            return jResult;
        }

        /// <summary>
        /// A move for the player
        /// </summary>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PlayerMove(string fromPosition, string toPosition)
        {
            var jResult = new JsonResult();
            return jResult;
        }

        public JsonResult ComputerMove()
        {
            return new JsonResult();
        }
    }
}