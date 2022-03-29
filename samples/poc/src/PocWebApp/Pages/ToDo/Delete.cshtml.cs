using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Poc.Entity;
using Poc.Repository;

using UpToDocu;

namespace PocWebApp.Pages.ToDo {
    public class DeleteModel : PageModel {
        private readonly PocRepository _PocRepository;
        private readonly UTDService _UtdService;

        public DeleteModel(
            PocRepository pocRepository,
            UTDService utdService

            ) {
            this._PocRepository = pocRepository;
            this._UtdService = utdService;
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

        public async Task<IActionResult> OnPostAsync(Guid? id) {
            this._UtdService + 
            UTD.ConditionalAsync(
                id.GetValueOrDefault(),
                (guidId) => (guidId == Guid.Empty),

                );
            var guidId = id.GetValueOrDefault();
            if (guidId == Guid.Empty) {
                return this.NotFound();
            } else {
                await this._PocRepository.TodoRepository.Delete(
                    new TodoItemDelete(
                        Id: guidId,
                        SerialVersion: string.Empty
                        ));
                return this.RedirectToPage("./Index");
            }
        }
    }
}
