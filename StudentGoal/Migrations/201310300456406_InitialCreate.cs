namespace StudentGoal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Goals",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        student_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Students", t => t.student_ID)
                .Index(t => t.student_ID);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        SkillID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Student_ID = c.Int(),
                    })
                .PrimaryKey(t => t.SkillID)
                .ForeignKey("dbo.Students", t => t.Student_ID)
                .Index(t => t.Student_ID);
            
            CreateTable(
                "dbo.Homework",
                c => new
                    {
                        HWID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.HWID);
            
            CreateTable(
                "dbo.SkillGoals",
                c => new
                    {
                        Skill_SkillID = c.Int(nullable: false),
                        Goal_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Skill_SkillID, t.Goal_Name })
                .ForeignKey("dbo.Skills", t => t.Skill_SkillID, cascadeDelete: true)
                .ForeignKey("dbo.Goals", t => t.Goal_Name, cascadeDelete: true)
                .Index(t => t.Skill_SkillID)
                .Index(t => t.Goal_Name);
            
            CreateTable(
                "dbo.HomeworkSkills",
                c => new
                    {
                        Homework_HWID = c.Int(nullable: false),
                        Skill_SkillID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Homework_HWID, t.Skill_SkillID })
                .ForeignKey("dbo.Homework", t => t.Homework_HWID, cascadeDelete: true)
                .ForeignKey("dbo.Skills", t => t.Skill_SkillID, cascadeDelete: true)
                .Index(t => t.Homework_HWID)
                .Index(t => t.Skill_SkillID);
            
            CreateTable(
                "dbo.HomeworkStudents",
                c => new
                    {
                        Homework_HWID = c.Int(nullable: false),
                        Student_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Homework_HWID, t.Student_ID })
                .ForeignKey("dbo.Homework", t => t.Homework_HWID, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_ID, cascadeDelete: true)
                .Index(t => t.Homework_HWID)
                .Index(t => t.Student_ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.HomeworkStudents", new[] { "Student_ID" });
            DropIndex("dbo.HomeworkStudents", new[] { "Homework_HWID" });
            DropIndex("dbo.HomeworkSkills", new[] { "Skill_SkillID" });
            DropIndex("dbo.HomeworkSkills", new[] { "Homework_HWID" });
            DropIndex("dbo.SkillGoals", new[] { "Goal_Name" });
            DropIndex("dbo.SkillGoals", new[] { "Skill_SkillID" });
            DropIndex("dbo.Skills", new[] { "Student_ID" });
            DropIndex("dbo.Goals", new[] { "student_ID" });
            DropForeignKey("dbo.HomeworkStudents", "Student_ID", "dbo.Students");
            DropForeignKey("dbo.HomeworkStudents", "Homework_HWID", "dbo.Homework");
            DropForeignKey("dbo.HomeworkSkills", "Skill_SkillID", "dbo.Skills");
            DropForeignKey("dbo.HomeworkSkills", "Homework_HWID", "dbo.Homework");
            DropForeignKey("dbo.SkillGoals", "Goal_Name", "dbo.Goals");
            DropForeignKey("dbo.SkillGoals", "Skill_SkillID", "dbo.Skills");
            DropForeignKey("dbo.Skills", "Student_ID", "dbo.Students");
            DropForeignKey("dbo.Goals", "student_ID", "dbo.Students");
            DropTable("dbo.HomeworkStudents");
            DropTable("dbo.HomeworkSkills");
            DropTable("dbo.SkillGoals");
            DropTable("dbo.Homework");
            DropTable("dbo.Skills");
            DropTable("dbo.Goals");
            DropTable("dbo.Students");
        }
    }
}
