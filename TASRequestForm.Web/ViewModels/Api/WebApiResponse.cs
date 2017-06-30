using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TASRequestForm.Web.ViewModels.Api
{
    public class WebApiResponse
    {
        public bool Success { get; set; }
        public string[] Messages { get; set; }
    }
}