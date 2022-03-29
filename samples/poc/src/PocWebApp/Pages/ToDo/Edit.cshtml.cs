using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Poc.Entity;
using Poc.Repository;

using System;
using System.Threading.Tasks;

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
            var result = await this._PocRepository.TodoRepository.Update(
                new TodoItemUpdate(
                    Id : this.ToDo.Id,
                    Title:this.ToDo.Title,
                    Done:  this.ToDo.Done,
                    SerialVersion:  this.ToDo.SerialVersion
                    ) 
                );
     
            return this.RedirectToPage("./Index");
        }
    }
}
