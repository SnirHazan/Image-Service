using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class LogsController : Controller
    {
        public static Logs LogsCollection = new Logs();
        public static List<string> types = new List<string>()
        {
            "FAIL","WARNING","INFO"
        };
        // GET: Configuration

        // GET: Logs
        public ActionResult LogsView()
        {
            return View(LogsCollection.LogList);
        }

        public ActionResult FilterLogsView(FormCollection filterByType)
        {
            string filter = filterByType["typeFilter"].ToString();
            if(filter == "" || filter == null)
            {
                return RedirectToAction("LogsView");
            }            
            ObservableCollection<Communication.MessageRecievedEventArgs> s = new ObservableCollection<Communication.MessageRecievedEventArgs>();
            foreach(Communication.MessageRecievedEventArgs e in LogsCollection.LogList)
            {
                if (e.Status.ToString() == filter)
                    s.Add(e);
            }
            return View(s);
        }
    }
}