using System.Web.Mvc;

namespace TASRequestForm.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_DeclinedRequests",
                "Admin/Requests/Declined/Page/{page}",
                new { controller = "Requests", action = "Declined" },
                new { page = @"^[1-9]\d*$" },
                new[] { "TASRequestForm.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_DeclinedRequests_FirstPage",
                "Admin/Requests/Declined",
                new { controller = "Requests", action = "Declined", page = 1 },
                new[] { "TASRequestForm.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_PendingRequests",
                "Admin/Requests/Pending/Page/{page}",
                new { controller = "Requests", action = "Pending" },
                new { page = @"^[1-9]\d*$" },
                new[] { "TASRequestForm.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_PendingRequests_FirstPage",
                "Admin/Requests/Pending",
                new { controller = "Requests", action = "Pending", page = 1 },
                new[] { "TASRequestForm.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_ApprovedRequests",
                "Admin/Requests/Approved/Page/{page}",
                new { controller = "Requests", action = "Approved" },
                new { page = @"^[1-9]\d*$" },
                new[] { "TASRequestForm.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_ApprovedRequests_FirstPage",
                "Admin/Requests/Approved",
                new { controller = "Requests", action = "Approved", page = 1 },
                new[] { "TASRequestForm.Web.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "TASRequestForm.Web.Areas.Admin.Controllers" }
            );
        }
    }
}