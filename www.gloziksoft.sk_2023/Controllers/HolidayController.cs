﻿using System.Web.Mvc;

namespace www.gloziksoft.sk_2023.Controllers
{
    public class HolidayController : _PgsoftwebController
    {
        public HolidayController()
            : base("Home")
        {
        }

        public ActionResult Iceland()
        {
            return View();
        }
        public ActionResult Rome()
        {
            return View();
        }

    }
}
