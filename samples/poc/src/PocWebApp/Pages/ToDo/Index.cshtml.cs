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
    public class IndexModel : PageModel
    {
        private readonly PocRepository _PocRepository;

        public IndexModel(PocRepository pocRepository)
        {
            this.ToDo = new List<TodoItem>();
            this._PocRepository = pocRepository;
        }

        public IList<TodoItem> ToDo { get;set; }

        public async Task OnGetAsync()
        {
            this.ToDo = await this._PocRepository.TodoRepository.GetList();
        }
    }
}
