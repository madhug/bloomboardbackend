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
    public class HomeworkController : Controller
    {
        private StudentGoalContext db = new StudentGoalContext();

        //
        // GET: /Homework/

        public ActionResult Index()
        {
            return View(db.Homework.ToList());
        }

        //
        // GET: /Homework/Details/5

        public ActionResult Details(int id = 0)
        {
            Homework homework = db.Homework.Find(id);
            if (homework == null)
            {
                return HttpNotFound();
            }
            return View(homework);
        }

        //
        // GET: /Homework/Create

        public ActionResult Create()
        {
            PopulateSkillsDropDownList();
            return View();
        }

        //
        // POST: /Homework/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Homework homework)
        {
            if (homework.skills == null)
            {
                homework.skills = new List<Skill>();
            }
            foreach (var s in homework.SkillsNeeded)
            {
                Skill sk = db.Skills.Find(s);
                homework.skills.Add(sk);
            }
            db.Homework.Add(homework);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Homework/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Homework homework = db.Homework.Find(id);
            if (homework == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        //
        // POST: /Homework/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Homework homework)
        {
            if (ModelState.IsValid)
            {
                db.Entry(homework).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(homework);
        }

        //
        // GET: /Homework/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Homework homework = db.Homework.Find(id);
            if (homework == null)
            {
                return HttpNotFound();
            }
            return View(homework);
        }

        //
        // POST: /Homework/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Homework homework = db.Homework.Find(id);
            db.Homework.Remove(homework);
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