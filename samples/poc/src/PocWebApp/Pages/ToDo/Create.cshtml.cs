using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Poc.Entity;

namespace PocWebApp.Pages.ToDo {
    public class CreateModel : PageModel {
        private readonly Poc.Entity.PocContext _context;

        public CreateModel(Poc.Entity.PocContext context) {
            this._context = context;
            this.TodoCreate = new TodoCreate();
        }

        public IActionResult OnGet() {
            return this.Page();
        }

        [BindProperty]
        public TodoCreate TodoCreate { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync() {
            if (!this.ModelState.IsValid) {
                return this.Page();
            }

            var todoCreate = this.TodoCreate;
            var now = System.DateTimeOffset.Now;
            var todo = new TodoEntity() {
                Id = Guid.NewGuid(),
                Title = todoCreate.Title,
                Done = todoCreate.Done,
                CreatedAt = now,
                ModifiedAt = now
            };
            this._context.Todo.Add(todo);
            await this._context.SaveChangesAsync();

            return this.RedirectToPage("./Index");
        }
    }
}
