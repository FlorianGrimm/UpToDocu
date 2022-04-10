using System.Runtime.CompilerServices;

namespace UpToDocu {
    public class UtdEnv : UtdObject {
        public UtdEnv(
                string name = "",
                UtdObject? kind = default,
                object? value = null,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath] string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0,
                params UtdObject[] props
            ) : base(
                name: name,
                kind:kind,
                value: value,
                callerMemberName: callerMemberName,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                props:props
            ) {
        }
    }
}