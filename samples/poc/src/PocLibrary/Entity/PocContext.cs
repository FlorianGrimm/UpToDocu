using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Entity {
    public class PocContext : DbContext {
        public PocContext() {
        }

        public virtual DbSet<TodoEntity> Todo { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<TodoEntity>(
                todoEntity => {
                    todoEntity.Property(e => e.Title);
                }
                );
            base.OnModelCreating(modelBuilder);

        }
    }
}
/*
 dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=SchoolDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models
 */