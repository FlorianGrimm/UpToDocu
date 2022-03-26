using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Poc.Entity;


namespace PocWebApp.Controller {
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoEntityController : ControllerBase {
        private readonly PocContext _context;

        public ToDoEntityController(PocContext context) {
            this._context = context;
        }

        // GET: ToDoEntity
        [HttpGet(Name = nameof(GetList))]
        public async Task<ActionResult<IEnumerable<TodoEntity>>> GetList() {
            return await this._context.Todo.ToListAsync();
        }

        // GET: ToDoEntity/Details/5
        [HttpGet("{id}", Name = nameof(GetItem))]
        public async Task<ActionResult<TodoEntity>> GetItem(Guid? id) {
            if (id == null) {
                return this.NotFound();
            } else {
                var toDoEntity = await this._context.Todo
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (toDoEntity == null) {
                    return this.NotFound();
                } else {
                    return toDoEntity;
                }
            }
        }

        // POST api/Todo
        [HttpPost(Name = nameof(PostItem))]
        public async Task<ActionResult<TodoEntity>> PostItem(
            [Bind("Id,Title,Done")]
            TodoCreate todoCreate
            ) {
            var now = System.DateTimeOffset.Now;
            var id = todoCreate.Id.GetValueOrDefault(Guid.Empty);
            if (id == Guid.Empty) { id = Guid.NewGuid(); }
            var todoEntity = new TodoEntity() {
                Id = id,
                Title = todoCreate.Title,
                Done = todoCreate.Done,
                CreatedAt = now,
                ModifiedAt = now
            };
            this._context.Todo.Add(todoEntity);
            await this._context.SaveChangesAsync();
            return this.CreatedAtAction(nameof(GetItem), new { id = todoEntity.Id }, todoEntity);
        }

        // PUT: _api/ToDo/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}", Name = nameof(PutItem))]
        public async Task<IActionResult> PutItem(
            Guid id,
            [Bind("Id,Title,Done")] TodoEntity todoEntity) {
            if (id != todoEntity.Id) {
                return this.NotFound();
            }

            try {
                // var toDoEntityCurrent = await _context.Todo.FirstOrDefaultAsync(m => m.Id == id);
                var toDoEntityCurrent = await this._context.Todo.FindAsync(id);
                if (toDoEntityCurrent is null) {
                    return this.NotFound();
                } else {
                    var now = System.DateTimeOffset.Now;

                    toDoEntityCurrent.Title = todoEntity.Title;
                    toDoEntityCurrent.Done = todoEntity.Done;
                    toDoEntityCurrent.ModifiedAt = now;
                    this._context.Update(todoEntity);
                    await this._context.SaveChangesAsync();
                    return this.RedirectToAction(nameof(Index));
                }
            } catch (DbUpdateConcurrencyException) {
                throw;
            }
        }

        // DELETE: _api/ToDo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoEntity>> DeleteConfirmed(Guid id) {
            var todoEntity = await this._context.Todo.FindAsync(id);
            if (todoEntity is null) {
                return this.NotFound();
            } else {
                this._context.Todo.Remove(todoEntity);
                await this._context.SaveChangesAsync();
                return this.Ok();
            }
        }
    }
}
