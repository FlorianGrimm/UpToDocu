using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UpToDocu {
    public class UTDSpecification : UTDObject {
        public Dictionary<string, UTDObject> PropsByCallerMemberName { get; }

        public UTDSpecification(
                string name = "",
                UTDObject? kind = default,
                object? value = null,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath] string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0,
                params UTDObject[] props
            ) : base(
                name: name,
                kind: kind,
                value: value,
                callerMemberName: callerMemberName,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber,
                props: props
            ) {
            this.PropsByCallerMemberName = new Dictionary<string, UTDObject>();
        }

        /*
        public UTDObject Register(
            UTDObject value
            ) {
            this.Props.Add(value);
            return value;
        }
        */

        public UTDObject Define(
            Func<UTDObject> fnValue,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            if (this.PropsByCallerMemberName.TryGetValue(callerMemberName, out var value)) {
                return value;
            } else {
                var reference = new UTDObject(name: "", kind: "reference");
                this.PropsByCallerMemberName.Add(callerMemberName, reference);
                value = fnValue();
                this.Props.Add(value);
                this.PropsByCallerMemberName[callerMemberName] = value;
                reference.Value = value;
                if (reference.Props.Count > 0) {
                    value.AddRange(reference);
                    reference.Props.Clear();
                }
                return value;
            }
        }

        public void Postpare() {
            foreach (var kv in this.PropsByCallerMemberName) {
                if (string.IsNullOrEmpty(kv.Value.Name)) {
                    kv.Value.Name = kv.Key;
                }
            }
        }

    }
}