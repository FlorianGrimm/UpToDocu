using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Poc.Entity;
using Poc.Repository;

namespace PocWebApp.Pages.ToDo {
    public class EditModel : PageModel {
        private readonly PocRepository _PocRepository;

        public EditModel(PocRepository pocRepository) {
            this._PocRepository = pocRepository;
            this.ToDo = new TodoItem();
        }

        [BindProperty]
        public TodoItem ToDo { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id) {
            var guidId = id.GetValueOrDefault();
            if (guidId == Guid.Empty) {
                return this.NotFound();
            } else {
                var toDo = await this._PocRepository.TodoRepository.GetItem(guidId);
                if (toDo == null) {
                    return this.NotFound();
                } else {
                    this.ToDo = toDo;
                    return this.Page();
                }
            }
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync() {
            if (!this.ModelState.IsValid) {
                return this.Page();
            }

            this._context.Attach(this.ToDo).State = EntityState.Modified;

            try {
                await this._context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!this.ToDoExists(this.ToDo.Id)) {
                    return this.NotFound();
                } else {
                    throw;
                }
            }

            return this.RedirectToPage("./Index");
        }

        private bool ToDoExists(Guid id) {
            return this._context.Todo.Any(e => e.Id == id);
        }
    }
}
