using System;

namespace Poc.Entity {
    public partial class TodoItem {
        public TodoItem() {
            this.Title = string.Empty;
            this.SerialVersion = string.Empty;
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public string SerialVersion { get; set; }
    }
    
}
