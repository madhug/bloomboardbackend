using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentGoal.Models;

namespace StudentGoal.Controllers
{
    public class GoalController : Controller
    {
        private StudentGoalContext db = new StudentGoalContext();

        //
        // GET: /Goal/

        public ActionResult Index()
        {
            //ModelState.Remove("skills");
            return View(db.Goals.Include(g => g.skills).ToList());
            //return View(db.Goals.ToList());
        }

        //
        // GET: /Goal/Details/5

        public ActionResult Details(int id)
        {
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        //
        // GET: /Goal/Create

        public ActionResult Create()
        {
            PopulateSkillsDropDownList();
            return View();
        }

        //
        // POST: /Goal/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Goal goal)
        {
            if (goal.skills == null)
            {
                goal.skills = new List<Skill>();
            }
            foreach (var s in goal.SkillsNeeded)
            {
                Skill sk = db.Skills.Find(s);
                goal.skills.Add(sk);
            }
            int studentID = (int)Session["currentStudent"];
            Student st = db.Students.Find(studentID);
            goal.student = st;
            db.Goals.Add(goal);
            db.SaveChanges();
            return RedirectToAction("Details", "Student", new { id = studentID });
        }

        //
        // GET: /Goal/Edit/5

        public ActionResult Edit(int id)
        {
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        //
        // POST: /Goal/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Goal goal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goal);
        }

        //
        // GET: /Goal/Delete/5

        public ActionResult Delete(int id)
        {
            Goal goal = db.Goals.Find(id);
            if (goal == null)
            {
                return HttpNotFound();
            }
            return View(goal);
        }

        //
        // POST: /Goal/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Goal goal = db.Goals.Find(id);
            db.Goals.Remove(goal);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void PopulateSkillsDropDownList(List<Skill> selectedSkills = null)
        {
            var skillsQuery = from s in db.Skills
                                orderby s.Name
                                select s;
            ViewBag.SkillsList = new MultiSelectList(skillsQuery.ToList(), "SkillID", "Name", selectedSkills);
        }
    }
}