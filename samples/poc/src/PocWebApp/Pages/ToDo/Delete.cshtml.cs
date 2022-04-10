using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Poc.Entity;
using Poc.Repository;

using UpToDocu;

namespace PocWebApp.Pages.ToDo {
    public class DeleteModel : PageModel {
        private readonly PocRepository _PocRepository;
        private readonly UtdService _UtdService;
        private readonly TodoItemDeleteRules _TodoItemDeleteRules;

        public DeleteModel(
            PocRepository pocRepository,
            UtdService utdService,
            TodoItemDeleteRules todoItemDeleteRules
            ) {
            this._PocRepository = pocRepository;
            this._UtdService = utdService;
            this._TodoItemDeleteRules = todoItemDeleteRules;
            this.ToDo = new TodoItem();
        }

        [BindProperty]
        public TodoItem ToDo { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id) {
            var idValue = this._TodoItemDeleteRules.ValidateId(id);
            var getItemValue = await idValue.ThenValueAsync(
                    ifValue: async (id, _) => await this._PocRepository.TodoRepository.GetItem(id)
                );
            return getItemValue.Result<IActionResult>(
                ifValue: (todoItem, _) => this.Page(),
                ifFailed: (_) => this.NotFound(),
                ifError: (error, _) => this.BadRequest(error.Message)
                );
        }


        public async Task<IActionResult> OnPostAsync(Guid? id) {
            var idValue = this._TodoItemDeleteRules.ValidateId(id);
            var todoItemDeleteValue = idValue.ThenValue(
                    ifValue: (id, _) => (
                        new TodoItemDelete(
                            Id: id,
                            SerialVersion: string.Empty
                            )
                    ).AsUtdValue(null!)
                );
            var deleteValue = await
                todoItemDeleteValue.ThenValueAsync(
                    ifValue: async (todoItemDelete, _) =>
                        await this._PocRepository.TodoRepository.Delete(todoItemDelete)
                );
            return deleteValue.Result<IActionResult>(
                ifValue: (_, _) => this.RedirectToPage("./Index"),
                ifFailed: (_) => this.NotFound(),
                ifError: (_, _) => this.RedirectToPage("./Error")
                );
            //var idValue = this._TodoItemDeleteRules.ValidateId(id);
            //var todoItemDeleteValue = idValue.ThenValue(
            //    ifValue: (id, _) => (
            //            new TodoItemDelete(
            //                Id: id,
            //                SerialVersion: string.Empty
            //                )
            //        ).AsUtdValue(null!)
            //    );
            //var deleteValue = await todoItemDeleteValue.ThenValueAsync(
            //    ifValue: async (todoItemDelete, _) =>
            //        await this._PocRepository.TodoRepository.Delete(todoItemDelete)
            //    );
            //return deleteValue.ThenValue(
            //    ifValue: (_, _) => this.RedirectToPage("./Index").AsUtdValue<IActionResult>(null!),
            //    ifFailed: (_) => this.NotFound().AsUtdValue<IActionResult>(null!),
            //    ifError: (_, _) => this.RedirectToPage("./Error").AsUtdValue<IActionResult>(null!)
            //    ).GetValue();
        }
    }

    //public partial class PocPipelines {
    //    private readonly IServiceProvider _ServiceProvider;

    //    public PocPipelines(IServiceProvider serviceProvider) {
    //        this._ServiceProvider = serviceProvider;

    //    }
    //    public IUtdPipeline<Guid, IActionResult> PipelineOnPostAsync() {
    //        var todoItemDeleteRules = this._ServiceProvider.GetRequiredService<TodoItemDeleteRules>();
    //        UtdPipelineBuilder<Guid> builder = new UtdPipelineBuilder<Guid>();
    //        var step1 = builder.Source().Then(
    //            UTD.UTDObject(""),
    //            id => todoItemDeleteRules.ValidateId(id)
    //            );
    //        var step2 = step1.If(
    //            UTD.UTDObject(""),
    //            ifValueAsync: id => async (todoItemDelete, _) =>
    //                  await this._PocRepository.TodoRepository.Delete(todoItemDelete)
    //            );
    //        return builder.Build(step3);
    //    }
    //}
}
