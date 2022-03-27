using Poc.Repository;

using System;

namespace Poc.Entity {
    public partial class TodoEntity {
        public TodoEntity() {
            this.Title = string.Empty;
            this.SerialVersion = SerialVersionConverter.GetEmptySerialVersion();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public byte[] SerialVersion { get; set; }
    }
}
