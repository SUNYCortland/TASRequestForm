using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TASRequestForm.Web.Areas.Admin.Controllers
{
    public class ErrorsController : Controller
    {
        public ErrorsController()
        {

        }

        //
        // GET: /Admin/Errors/NotAuthorized
        public ActionResult NotAuthorized()
        {
            return View();
        }
    }
}