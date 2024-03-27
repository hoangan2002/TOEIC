using ChillToeic.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace ChillToeic.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<EducationCenter> EducationCenters { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<LectureDetail> LectureDetails { get; set; }
        public DbSet<LectureType> LectureTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionDetail> QuestionDetails { get; set; }
        public DbSet<QuestionOfTest> QuestionOfTests { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestOfUser> TestOfUsers { get; set; }
        public DbSet<TestType> TestTypes { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
