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
    public class IndexModel : PageModel
    {
        private readonly Poc.Entity.PocContext _context;

        public IndexModel(Poc.Entity.PocContext context)
        {
            this._context = context;
            this.ToDo = new List<TodoEntity>();
        }

        public IList<TodoEntity> ToDo { get;set; }

        public async Task OnGetAsync()
        {
            this.ToDo = await this._context.Todo.ToListAsync();
        }
    }
}
