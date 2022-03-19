using UpToDocu;
namespace Poc.Consideration {
    public class Step1 : UTDSpecification {
        public Step1() : base() {
        }

        public UTDObject SqlServer => Define(() => "MS SQL");

        public UTDObject Database => Define(() => SqlServer / "TodoDB");
    }
}
