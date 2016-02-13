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
            return View();
        }
    }
}