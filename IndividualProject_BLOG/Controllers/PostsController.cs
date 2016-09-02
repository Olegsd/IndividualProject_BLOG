using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Security.Authentication.ExtendedProtection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using IndividualProject_BLOG.Models;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace IndividualProject_BLOG.Controllers
{
    [ValidateInput(false)]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            var post = db.Posts.Include(p => p.Author).ToList();
            
            return View(post);
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            post.Author = db.Users.Find(post.Author_Id);
            post.Comments = db.Comments.Include(c => c.Posts).Include(c => c.Author).Include(c => c.Posts).Where(c => c.Posts.Id == post.Id).OrderByDescending(c => c.Date);

            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }


        // GET: Posts/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            var authors = db.Users.ToList();
            ViewBag.Authors = authors; 
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrators")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,Date,Author_Id")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [Authorize(Roles = "Administrators")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Comments Controllers

        // GET: Comments
        public ActionResult IndexComm()
        {
            var comments = db.Comments.Include(c => c.Author).Include(c => c.Posts).OrderByDescending(d => d.Posts.Date).ThenBy(t => t.Posts.Title);

            ViewBag.Author = new SelectList(db.Users, "Id", "FullName", "UserName");
            return View(comments);
        }

        // GET: Comments/Details/5
        public ActionResult DetailsComm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comment = db.Comments.Find(id);
            comment.Author = db.Users.Find(comment.Author_Id);

            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        [Authorize]
        public ActionResult CreateComm(int? id)
        {
            var comment = db.Comments.Find(id);
            ViewBag.Author_Id = new SelectList(db.Users, "Id", "FullName");
            return View(comment);
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComm([Bind(Include = "Id,Text")] Comment comment, int? id)
        {
            if (ModelState.IsValid)
            {
                comment.Author = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                comment.Posts = db.Posts.FirstOrDefault(p => p.Id == id);
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = comment.Posts_Id });
            }

            ViewBag.Author_Id = new SelectList(db.Users, "Id", "FullName", comment.Author_Id);
            return View(comment);
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Administrators")]
        public ActionResult EditComm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Author_Id = new SelectList(db.Users, "Id", "FullName", comment.Author_Id);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrators")]
        [ValidateAntiForgeryToken]
        public ActionResult EditComm([Bind(Include = "Id,Text,Date,Author_Id,Posts_Id")] Comment comment, int? id)
        {
            if (ModelState.IsValid)
            {
                //comment.Posts = db.Posts.FirstOrDefault(p => p.Id == id);
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new {id = comment.Posts_Id });
            }
            ViewBag.Author_Id = new SelectList(db.Users, "Id", "FullName", comment.Author_Id);
            return View(comment);
        }
        // GET: Comments/Delete/5
        [Authorize(Roles = "Administrators")]
        public ActionResult DeleteComm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("DeleteComm")]
        [Authorize(Roles = "Administrators")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedComm(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = comment.Posts_Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
