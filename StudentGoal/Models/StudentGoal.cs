using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentGoal.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int[] HomeworkAssigned { get; set; }
        public virtual ICollection<Goal> goals { get; set; }
        public virtual ICollection<Homework> homeworks { get; set; }
        public virtual ICollection<Skill> skills { get; set; }
    }

    public class Goal
    {
        [Key]
        public int GoalID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int[] SkillsNeeded { get; set; }
        public virtual Student student { get; set; }
        public virtual ICollection<Skill> skills { get; set; }
    }

    public class Skill
    {
        [Key]
        public int SkillID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Goal> goals { get; set; }
        public virtual ICollection<Homework> homeworks { get; set; }
    }

    public class Homework
    {
        [Key]
        public int HWID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int[] SkillsNeeded { get; set; }
        public virtual ICollection<Skill> skills { get; set; }
        public virtual ICollection<Student> students { get; set; }
    }
}