using Microsoft.EntityFrameworkCore;


namespace TaskManagerAPI.Models
{
    public class TaskManagerDbContext : DbContext
    {
        public TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options)
            : base(options)
        {
        }

        // Define DbSets for each table
        public DbSet<User> Users { get; set; }
        public DbSet<UserTask> Tasks { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        // Override OnModelCreating if needed (e.g., configure relationships, indices)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User table
            modelBuilder.Entity<User>().ToTable("Users");

            // Configure Task table with a foreign key to User
            modelBuilder.Entity<UserTask>()
                .ToTable("Tasks")
                .HasKey(t => t.TaskId);

            modelBuilder.Entity<UserTask>()
                .HasIndex(t => t.UserId); // Create index for UserId for performance

            modelBuilder.Entity<UserTask>()
                .HasOne<User>() // No navigation property, so we specify the type explicitly
                .WithMany() // No navigation property on the other side
                .HasForeignKey(t => t.UserId) // Define the foreign key
                .OnDelete(DeleteBehavior.Cascade); // Optional: Configure delete behavior

            // Configure TaskHistory table with a foreign key to Task
            modelBuilder.Entity<TaskHistory>()
                .ToTable("TaskHistories")
                .HasKey(th => th.HistoryId);

            modelBuilder.Entity<TaskHistory>()
                .HasIndex(th => th.TaskId); // Create index for TaskId for performance

            modelBuilder.Entity<TaskHistory>()
                .HasOne<UserTask>() // No navigation property
                .WithMany() // No navigation property on the other side
                .HasForeignKey(th => th.TaskId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete

            // Configure Notification table with foreign keys to User and Task
            modelBuilder.Entity<Notification>()
                .ToTable("Notifications")
                .HasKey(n => n.NotificationId);

            modelBuilder.Entity<Notification>()
                .HasIndex(n => new { n.UserId, n.TaskId }); // Composite index for performance

            modelBuilder.Entity<Notification>()
                .HasOne<User>() // Foreign key to User
                .WithMany() // No navigation property
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Optional: Configure delete behavior

            modelBuilder.Entity<Notification>()
                .HasOne<UserTask>() // Foreign key to Task
                .WithMany() // No navigation property
                .HasForeignKey(n => n.TaskId)
                .OnDelete(DeleteBehavior.Restrict); // Optional: Configure delete behavior
        }

    }
}
