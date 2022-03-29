using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Poc.Entity;
using Poc.Repository;

using UpToDocu;

namespace PocWebApp.Controller {
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoEntityController : ControllerBase {
        private readonly PocRepository _PocRepository;
        private readonly UTDService _UTDService;

        public ToDoEntityController(
            PocRepositoryScoped pocRepositoryScoped,
            UpToDocu.UTDService utdService
            ) {
            this._PocRepository = pocRepositoryScoped.PocRepository;
            this._UTDService = utdService;
        }

        // GET: ToDoEntity
        [HttpGet(Name = nameof(GetList))]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetList() {
            return await this._PocRepository.TodoRepository.GetList();
        }

        // GET: ToDoEntity/Details/5
        [HttpGet("{id}", Name = nameof(GetItem))]
        public async Task<ActionResult<TodoItem>> GetItem(Guid? id) {
            return await UTD.ConditionalAsync<Guid, ActionResult<TodoItem>>(
                this._UTDService + Spec.SpecificationPocWebApp.Instance.ToDoEntityController_GetItem,
                id.GetValueOrDefault(),
                id => id != Guid.Empty,
                ifTrue: async (id) => {
                    var result = await this._PocRepository.TodoRepository.GetItem(id);
                    if (result is not null) {
                        return result;
                    } else {
                        return this.NotFound();
                    }
                },
                ifFalse: (id) => this.NotFound()
                );
            if (id.HasValue) {
                var result = await this._PocRepository.TodoRepository.GetItem(id.Value);
                if (result is not null) {
                    return result;
                }
            } else { 
                return this.NotFound();
            }
        }

        // POST api/Todo
        [HttpPost(Name = nameof(PostItem))]
        public async Task<ActionResult<TodoItem>> PostItem(
            [Bind("Id,Title,Done")]
            TodoItemCreate todoCreate
            ) {
            var todoEntity = await this._PocRepository.TodoRepository.Insert(todoCreate);
            if (todoEntity is null) {
                return this.Conflict();
            } else {
                return this.CreatedAtAction(nameof(GetItem), new { id = todoEntity.Id }, todoEntity);
            }
        }

        // PUT: _api/ToDo/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut("{id}", Name = nameof(PutItem))]
        public async Task<ActionResult<TodoItem>> PutItem(
            Guid id,
            [Bind("Id,Title,Done,SerialVersion")] TodoItemUpdate todoItem
            ) {
            if (todoItem.Id == Guid.Empty) { todoItem = todoItem with { Id = id }; }
            if (id == Guid.Empty) { id = todoItem.Id; }
            if (id != todoItem.Id) { return this.NotFound(); }

            var result = await this._PocRepository.TodoRepository.Update(todoItem);
            if (result is null) {
                return this.NotFound();
            } else {
                return this.Ok(result);
            }
        }

        // DELETE: _api/ToDo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id, [Bind("Id,SerialVersion")] TodoItemDelete todoItemDelete) {
            if (todoItemDelete.Id == Guid.Empty) {
                todoItemDelete = todoItemDelete with { Id = id };
            }
            if (id == Guid.Empty) { id = todoItemDelete.Id; }
            var result = await this._PocRepository.TodoRepository.Delete(todoItemDelete);
            if (result) {
                return this.Ok();
            } else {
                return this.NotFound();
            }
        }
    }
}
