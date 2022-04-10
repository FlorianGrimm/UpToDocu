using System;

using UpToDocu;

namespace Poc.Entity {
    /*
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
    */
    public record class TodoItem() {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool Done { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public string SerialVersion { get; set; } = string.Empty;
    }

    public record class TodoItemGet(
        Guid Id
        );

    public record class TodoItemCreate(
        Guid? Id,
        string Title,
        bool Done
        );

    public record class TodoItemUpdate(
        Guid Id,
        string Title,
        bool Done,
        string SerialVersion
        );

    public record class TodoItemDelete(
        Guid Id,
        string SerialVersion
        );

    public class TodoItemDeleteRulesSpec : UpToDocu.UtdSpecification {
        public static TodoItemDeleteRulesSpec Instance => GetInstance(() => new TodoItemDeleteRulesSpec());

        public UtdObject ConditionId => this.Define(
            () => UTD.UTDObject("id is not empty", SpecificationCommon.Condition)
            );

    }
    public class TodoItemDeleteRules : UpToDocu.UtdSpecification {
        private readonly UtdBound  ConditionId;
        private readonly UtdService _UTDService;

        public TodoItemDeleteRules(UtdService utdService) {
            this._UTDService = utdService;
            this.ConditionId = utdService + TodoItemDeleteRulesSpec.Instance.ConditionId;
        }

        public UtdValue<Guid> ValidateId(Guid? id) {
            return UtdValue.ConditionalValue(
                this.ConditionId,
                id.GetValueOrDefault(),
                id => id != Guid.Empty
                );
        }
    }
}
