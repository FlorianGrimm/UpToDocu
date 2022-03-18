using System;
using System.Runtime.CompilerServices;

namespace UpToDocu {
    public class UTDSpecification : UTDObject {

        public UTDSpecification(
                string name = "",
                object? value = null,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath] string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0,
                params UTDObject[] props
            ) : base(
                name: name,
                value: value,
                callerMemberName: callerMemberName,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                props: props
            ) {
        }

        public UTDObject Define(UTDObject value) {
            this.Props.Add(value);
            return value;
        }
    }
}