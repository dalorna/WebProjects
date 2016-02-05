using CheckersWeb.Classes;
using CheckersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CheckersWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var game = new Game();
            var model = new BoardViewModel
            {
                Pieces = game.InitBoard()
            };
            return View(model);
        }
    }
}