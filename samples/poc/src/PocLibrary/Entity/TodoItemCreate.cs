using System;

namespace Poc.Entity {
    public partial class TodoItemCreate {
        public TodoItemCreate() {
            this.Title = string.Empty;
        }
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
    }

}
