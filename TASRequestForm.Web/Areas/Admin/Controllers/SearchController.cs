using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Services;

namespace TASRequestForm.Web.Areas.Admin.Controllers
{
    public class SearchController : Controller
    {
        private readonly ITestTimeService _testTimeService;

        public SearchController(ITestTimeService testTimeService)
        {
            _testTimeService = testTimeService;
        }

        // GET: /Admin/Search?query={query}
        public ActionResult Index(string query)
        {
            var results = new List<TestTime>();

            if (!String.IsNullOrEmpty(query))
            {
                results = _testTimeService.Search(query).ToList();
            }

            return View(results);
        }
    }
}