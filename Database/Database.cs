using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Database
{
    public class Database : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-DMKSNC5\SQLEXPRESS;Initial Catalog=KursDatabase;Integrated Security=True;MultipleActiveResultSets=True;");

             }
            base.OnConfiguring(optionsBuilder);
        }
        public virtual DbSet<Course> Courses { set; get; }
        public virtual DbSet<Education> Educations { set; get; }
        public virtual DbSet<EducationCourse> EducationCourses { set; get; }
        public virtual DbSet<Client> Clients { set; get; }
        public virtual DbSet<Payment> Payments { set; get; }
    }
}
