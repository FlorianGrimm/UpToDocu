using Microsoft.EntityFrameworkCore;

using Poc.Entity;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Repository {
    public class TodoRepository : IDisposable {
        private PocRepository? _Repository;
        public PocRepository Repository => _Repository ?? throw new ObjectDisposedException(nameof(Repository));

        private readonly SerialVersionConverter _SerialVersionConverter;

        public TodoRepository(PocRepository pocRepository) {
            this._Repository = pocRepository;
            this._SerialVersionConverter = new SerialVersionConverter();
        }

        [return: NotNullIfNotNull("e")]
        private TodoItem? ConvertFromEntity(TodoEntity? e) {
            if (e is null) {
                return null;
            } else {
                return new TodoItem() {
                    Id = e.Id,
                    Title = e.Title,
                    Done = e.Done,
                    CreatedAt = e.CreatedAt,
                    ModifiedAt = e.ModifiedAt,
                    SerialVersion = this._SerialVersionConverter.ConvertFromEntity(e.SerialVersion)
                };
            }
        }

        public async Task<List<TodoItem>> GetList() {
            var resultEntity = await this.Repository.Context.Todo.ToListAsync();
            return resultEntity.Select(e => this.ConvertFromEntity(e)).ToList();
        }

        public async Task<TodoItem?> GetItem(Guid id) {
            var resultEntity = await this.Repository.Context.Todo.FindAsync(id);
            return this.ConvertFromEntity(resultEntity);
        }

        public async Task<TodoItem?> Insert(TodoItemCreate todoCreate) {
            var now = System.DateTimeOffset.Now;
            var context = this.Repository.Context;
            var id = todoCreate.Id.GetValueOrDefault(Guid.Empty);
            if (id == Guid.Empty) {
                id = Guid.NewGuid();
            } else {
                var existingEntity = await context.Todo.FindAsync(id);
                if (existingEntity is not null) {
                    throw new InvalidOperationException();
                }
            }
            var todoEntity = new TodoEntity() {
                Id = id,
                Title = todoCreate.Title,
                Done = todoCreate.Done,
                CreatedAt = now,
                ModifiedAt = now
            };
            context.Todo.Add(todoEntity);
            await context.SaveChangesAsync();
            return this.ConvertFromEntity(todoEntity);
        }

        public async Task<TodoItem?> Update(TodoItemUpdate todoEntity) {
            var context = this.Repository.Context;
            var id = todoEntity.Id;
            var toDoEntityCurrent = await context.Todo.FindAsync(id);
            if (toDoEntityCurrent is null) {
                //return this.NotFound();
                throw new InvalidOperationException();
            } else {
                var now = System.DateTimeOffset.Now;
                if (string.Equals(toDoEntityCurrent.Title, todoEntity.Title, StringComparison.Ordinal)
                    && (toDoEntityCurrent.Done == todoEntity.Done)
                    ) {
                    return null;
                } else {
                    toDoEntityCurrent.Title = todoEntity.Title;
                    toDoEntityCurrent.Done = todoEntity.Done;
                    toDoEntityCurrent.ModifiedAt = now;
                    context.Update(toDoEntityCurrent);
                    await context.SaveChangesAsync();
                    //return this.RedirectToAction(nameof(Index));
                    return this.ConvertFromEntity(toDoEntityCurrent)!;
                }
            }
            /*
            try {
            } catch (DbUpdateConcurrencyException) {
                throw;
            }             
             */
        }

        public async Task<bool> Delete(TodoItemDelete todoItemDelete) {
            var context = this.Repository.Context;

            var todoEntity = await context.Todo.FindAsync(todoItemDelete.Id);
            if (todoEntity is null) {
                return false;
            } else {
                if (
                    !string.IsNullOrEmpty(todoItemDelete.SerialVersion)
                    && (this._SerialVersionConverter.EqualsSerialVersion(
                        todoEntity.SerialVersion,
                        this._SerialVersionConverter.ConvertToEntity(todoItemDelete.SerialVersion)
                        ))
                    ) {
                    return false;
                } else {
                    context.Todo.Remove(todoEntity);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
        }

        protected virtual void Dispose(bool disposing) {
            this._Repository = null;
        }

        ~TodoRepository() {
            this.Dispose(disposing: false);
        }

        public void Dispose() {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
