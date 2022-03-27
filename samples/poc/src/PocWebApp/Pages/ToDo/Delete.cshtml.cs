using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Poc.Entity;
using Poc.Repository;

namespace PocWebApp.Pages.ToDo {
    public class DeleteModel : PageModel {
        private readonly PocRepository _PocRepository;

        public DeleteModel(
            PocRepository pocRepository
            ) {
            this._PocRepository = pocRepository;
            this.ToDo = new TodoEntity();
        }

        [BindProperty]
        public TodoEntity ToDo { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id) {
            if (!id.HasValue) {
                return this.NotFound();
            } else {
                var toDo = await this._PocRepository.TodoRepository.GetItem(id);
                if (toDo == null) {
                    return this.NotFound();
                } else {
                    this.ToDo = toDo;
                    return this.Page();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid? id) {
            if (id == null) {
                return this.NotFound();
            }
            await this._PocRepository.TodoRepository.Delete(new TodoItemDelete() { Id=id});
            return this.RedirectToPage("./Index");
        }
    }
}