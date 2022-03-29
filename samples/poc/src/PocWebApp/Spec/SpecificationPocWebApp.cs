using System.Runtime.CompilerServices;

using UpToDocu;

namespace PocWebApp.Spec {
    public class SpecificationPocWebApp : UpToDocu.UTDSpecification {
        public static SpecificationPocWebApp Instance => UpToDocu.UTDSpecification.GetInstance(() => new SpecificationPocWebApp());
        /*
        public SpecificationPocWebApp(
                string name = "",
                UTDObject? kind = default,
                object? value = null,
                //[CallerMemberName] string callerMemberName = "",
                //[CallerFilePath] string callerFilePath = "",
                //[CallerLineNumber] int callerLineNumber = 0,
                params UTDObject[] props
            ) : base(
                name: name,
                kind: kind,
                value: value,
                //callerMemberName: callerMemberName,
                //callerFilePath: callerFilePath,
                //callerLineNumber: callerLineNumber,
                props: props
            ) {
        }
    */
        public SpecificationPocWebApp() : base() {
        }

        public UTDObject ToDoEntityController => this.Define(
            () => UTD.UTDClass<PocWebApp.Controller.ToDoEntityController>()
            );

        public UTDObject ToDoEntityController_GetItem => this.Define(
            () => ToDoEntityController / UTD.UTDMethod(nameof(PocWebApp.Controller.ToDoEntityController.GetItem))
            );
    }
}
