using CodeArtCommandsExperiment.CodeArtCommand;
using DeveloperTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DeveloperTools.Controllers
{
    public class ConsoleController : DeveloperToolsController
    {
        private readonly CommandManager _manager;

        public ConsoleController(CommandManager cman)
        {
            _manager = cman;
        }


        public ActionResult Index()
        {
            return View(new ConsoleModel());
        }

        public ActionResult FetchLog(int LastLogNo)
        {
            _manager.UpdateJobs();
            var lst = _manager.Log.Skip(LastLogNo).Take(100).ToList();
            return Json(new { LastNo = LastLogNo + lst.Count, LogItems = lst }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RunCommand(string command)
        {
            _manager.ExecuteCommand(command);
            return Json(new { });
        }
    }
}
