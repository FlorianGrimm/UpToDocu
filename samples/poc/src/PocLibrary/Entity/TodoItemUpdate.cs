using System;

namespace Poc.Entity {
    public partial class TodoItemUpdate {
        public TodoItemUpdate() {
            this.Title = string.Empty;
            this.SerialVersion = string.Empty;
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public string SerialVersion { get; set; }
    }
}
