using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IndividualProject_BLOG.Models;

namespace IndividualProject_BLOG.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            var post = db.Posts.OrderByDescending(p => p.Date).Take(4);
            return View(post.ToList());
        }
    }
}