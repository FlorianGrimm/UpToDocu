using System;

namespace Poc.Entity {
    public partial class TodoItemDelete {
        public TodoItemDelete() {
            this.SerialVersion = string.Empty;
        }
        public Guid Id { get; set; }
        public string SerialVersion { get; set; }
    }
}
