using System.Data.Entity;
using StudentGoal.Migrations;

namespace StudentGoal.Models
{
    public class StudentGoalContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<StudentGoal.Models.StudentGoalContext>());

        public StudentGoalContext() : base("name=StudentGoalContext")
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Goal> Goals { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<Homework> Homework { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<StudentGoalContext>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<StudentGoalContext, Configuration>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<QuizContext>());
        }
    }
}
