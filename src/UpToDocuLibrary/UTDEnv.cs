using System.Runtime.CompilerServices;

namespace UpToDocu {
    public class UTDEnv : UTDObject {
        public UTDEnv(
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
                props:props
            ) {
        }
    }
}