using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UpToDocu {
    public class UTDSpecification : UTDObject {
        static UTDSpecification() {
            _Specifications = new Dictionary<string, UTDSpecification>(StringComparer.Ordinal);
        }

        private static Dictionary<string, UTDSpecification> _Specifications;
        public static T GetInstance<T>(Func<T> creation)
            where T : UTDSpecification {
            var key = typeof(T).FullName!;
            if (_Specifications.TryGetValue(key, out var existing)) {
                return (T)existing;
            } else {
                var result = creation();
                _Specifications[key] = result;
                result.Postpare();
                return result;
            }
        }

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
            _Specifications[this.GetType().FullName!] = this;
            this.PropsByCallerMemberName = new Dictionary<string, UTDObject>();
            if (string.IsNullOrEmpty(this.Name)) { this.Name = this.GetType().FullName!; }
            this.Kind = UpToDocu.Spec.SpecificationCommon.Instance;
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
            foreach (var property in this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)) {
                if (typeof(UpToDocu.UTDObject).Equals(property.DeclaringType)) {
                    continue;
                }
                if (property.CanRead) {
                    if (property.PropertyType.IsAssignableTo(typeof(UTDObject))) {
                        if (property.GetValue(this) is UTDObject utdObject) {
                            if (string.IsNullOrEmpty(utdObject.Name)) {
                                utdObject.Name = property.Name;
                            }
                            if (!this.PropsByCallerMemberName.ContainsKey(utdObject.Name)) {
                                this.PropsByCallerMemberName[utdObject.Name] = utdObject;
                            }
                        }
                    }
                }
            }
            foreach (var method in this.GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)) {
                if (typeof(UpToDocu.UTDObject).Equals(method.DeclaringType)) {
                    continue;
                }
                if (typeof(UpToDocu.UTDObject).IsAssignableFrom(method.ReturnType)) {
                    if (method.GetParameters().Length == 0) {
                        var methodResult = method.Invoke(obj: this, parameters: Array.Empty<object>());
                        if (methodResult is UTDObject utdObject) {
                            if (string.IsNullOrEmpty(utdObject.Name)) {
                                utdObject.Name = method.Name;
                            }
                            if (!this.PropsByCallerMemberName.ContainsKey(utdObject.Name)) {
                                this.PropsByCallerMemberName[utdObject.Name] = utdObject;
                            }
                        }
                    }
                }
            }

        }
    }
}