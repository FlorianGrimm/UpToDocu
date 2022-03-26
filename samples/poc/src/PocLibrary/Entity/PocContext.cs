using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Poc.Entity.PocContext
namespace Poc.Entity {
    public partial class PocContext : DbContext {
        public PocContext() {
            this.Todo = Set<TodoEntity>();
        }
        public PocContext(DbContextOptions<PocContext> options)
            : base(options) {
            this.Todo = Set<TodoEntity>();
        }

        public virtual DbSet<TodoEntity> Todo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseInMemoryDatabase("Test");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TodoEntity>(
                todoEntity => {
                    todoEntity.Property(e => e.Id).IsRequired().ValueGeneratedNever();
                    todoEntity.Property(e => e.Title).HasMaxLength(255);
                    todoEntity.Property(e => e.Done);
                    todoEntity.Property(e => e.CreatedAt).IsRequired();
                    todoEntity.Property(e => e.ModifiedAt).IsRequired();

                    todoEntity.Property(e => e.SerialVersion)
                        .IsRequired()
                        .IsRowVersion()
                        .IsConcurrencyToken();

                    todoEntity.HasKey(nameof(TodoEntity.Id));
                    todoEntity.ToTable("Todo");
                }
                );
            base.OnModelCreating(modelBuilder);

        }
    }
}
/*
 dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models
 */