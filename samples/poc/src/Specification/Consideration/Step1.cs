using UpToDocu;
namespace Poc.Consideration {
    public class Step1 : UtdSpecification {
        public Step1() : base() {
        }

        public UtdObject SqlServer => Define(() => "MS SQL");

        public UtdObject Database => Define(() => SqlServer / "TodoDB");
    }
}
