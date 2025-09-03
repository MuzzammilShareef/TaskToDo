using Microsoft.EntityFrameworkCore;
using TaskToDo.DTO;
using TaskToDo.Models;

namespace TaskToDo.Data
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }

        // Keep DbSet plural: because its a table and it contains multiple records
        public DbSet<List> Lists { get; set; }
        public DbSet<TaskToDo.Models.Task> Tasks { get; set; }
        public DbSet<SeeDataDto> SeeDataDtos { get; set; }  // For mapping stored procedure results

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map Lists DbSet to actual 'List' table
            modelBuilder.Entity<List>().ToTable("List");
            modelBuilder.Entity<TaskToDo.Models.Task>().ToTable("Task");

            modelBuilder.Entity<List>()
                .HasMany(l => l.Tasks)
                .WithOne(t => t.List)
                .HasForeignKey(t => t.ListID)
                .OnDelete(DeleteBehavior.Cascade);

            // Since SeeDataDto is for raw SQL projection, mark it as a keyless entity
            modelBuilder.Entity<SeeDataDto>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(null);    // It is not mapped to a table or view
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
