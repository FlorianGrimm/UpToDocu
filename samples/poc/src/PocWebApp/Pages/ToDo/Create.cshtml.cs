using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using Poc.Entity;
using Poc.Repository;

namespace PocWebApp.Pages.ToDo {
    public class CreateModel : PageModel {
        private readonly PocRepository _PocRepository;

        public CreateModel(
            PocRepository pocRepository
            ) {
            this._PocRepository = pocRepository;
            this.TodoCreate = new TodoItemCreate(
                Id: Guid.Empty,
                Title: string.Empty,
                Done: false
                );
        }

        public IActionResult OnGet() {
            return this.Page();
        }

        [BindProperty]
        public TodoItemCreate TodoCreate { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync() {
            if (!this.ModelState.IsValid) {
                return this.Page();
            }
            await this._PocRepository.TodoRepository.Insert(this.TodoCreate);
            return this.RedirectToPage("./Index");
        }
    }
}
