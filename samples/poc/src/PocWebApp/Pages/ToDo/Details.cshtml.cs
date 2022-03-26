using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Poc.Entity;

namespace PocWebApp.Pages.ToDo
{
    public class DetailsModel : PageModel
    {
        private readonly Poc.Entity.PocContext _context;

        public DetailsModel(Poc.Entity.PocContext context)
        {
            this._context = context;
            this.ToDo = new TodoEntity();
        }

        public TodoEntity ToDo { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var toDo = await this._context.Todo.FirstOrDefaultAsync(m => m.Id == id);

            if (toDo == null) {
                return this.NotFound();
            } else {
                this.ToDo = toDo;
                return this.Page();
            }
        }
    }
}
