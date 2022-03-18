using System.Runtime.CompilerServices;

namespace UpToDocu {
    public static class UTD {
        public static UTDObject UTDObject(
            string name = "",
            object? value = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            return new UTDObject(
                name: name,
                value:value,
                callerMemberName: callerMemberName,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber);
        }
    }
}