using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentGoal.Models;
using System.Reflection;

namespace StudentGoal.Controllers
{
    public class StudentController : Controller
    {
        private StudentGoalContext db = new StudentGoalContext();

        //
        // GET: /Student/

        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        //
        // GET: /Student/Details/5

        public ActionResult Details(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            Session["currentStudent"] = id;
            return View(student);
        }

        //
        // GET: /Student/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Student/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        //
        // GET: /Student/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // POST: /Student/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        //
        // GET: /Student/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // POST: /Student/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [HttpGet, ActionName("Assign")]
        public ActionResult Assign()
        {
            PopulateHomeworkDropDownList();
            Student student = db.Students.Find(Session["currentStudent"]);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Assign")]
        public ActionResult Assign(Student student)
        {
            Student st = db.Students.Find(student.ID);
            if (student.homeworks == null)
            {
                st.homeworks = new List<Homework>();
            }
            if (student.HomeworkAssigned != null)
            {
                st.HomeworkAssigned = student.HomeworkAssigned;
                foreach (var h in student.HomeworkAssigned)
                {
                    Homework hw = db.Homework.Find(h);
                    st.homeworks.Add(hw);
                    db.Entry(hw).State = EntityState.Modified;
                    hw.students.Add(st);
                }
                db.Entry(st).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Details",new {id = st.ID});
        }

        private void PopulateHomeworkDropDownList(List<Homework> selectedHW = null)
        {
            Student current = db.Students.Find(Session["currentStudent"]);
            /*var skills_needed = from s in db.Skills
                                where (current.goals)*/
                                    
            /*var skillsQuery = from g in current.goals
                              join s in db.Skills
                              on g.skills.Contains(s)
                              select s;*/

            var skillsQuery = from s in db.Skills.Include(s => s.goals)
                              where s.goals.Any(g => g.skills.Contains(s))
                              select s;

            var homeworkQuery = from hw in db.Homework.Include(hw => hw.skills)
                                where hw.skills.Any(s => s.homeworks.Contains(hw))
                                select hw;

            List<Homework> homeworks = homeworkQuery.ToList();
            ViewBag.HomeworkList = new MultiSelectList(homeworks, "HWID", "Name", selectedHW);
        }
    }

    public class RequireRequestValueAttribute : ActionMethodSelectorAttribute
    {
        public RequireRequestValueAttribute(string valueName)
        {
            ValueName = valueName;
        }
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return (controllerContext.HttpContext.Request[ValueName] != null);
        }
        public string ValueName { get; private set; }
    }
}