namespace Assignment1
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AssignmentModelContext : DbContext
    {
        public AssignmentModelContext()
            : base("name=AssignmentModelContext")
        {
        }

        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<LearningEvent> LearningEvents { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Register> Registers { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Instructor>()
                .HasMany(e => e.Modules)
                .WithRequired(e => e.Instructor)
                .HasForeignKey(e => e.InsturctorID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LearningEvent>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.LearningEvent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
                .HasMany(e => e.LearningEvents)
                .WithRequired(e => e.Module)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
                .HasMany(e => e.Registers)
                .WithRequired(e => e.Module)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Attendances)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(e => e.Registers)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentID)
                .WillCascadeOnDelete(false);
        }
    }
}
