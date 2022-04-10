using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UpToDocu {
    public class UtdSpecification : UtdObject {
        static UtdSpecification() {
            _Specifications = new Dictionary<string, UtdSpecification>(StringComparer.Ordinal);
        }

        private static Dictionary<string, UtdSpecification> _Specifications;
        public static T GetInstance<T>(Func<T> creation)
            where T : UtdSpecification {
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

        public Dictionary<string, UtdObject> PropsByCallerMemberName { get; }

        public UtdSpecification(
                string name = "",
                UtdObject? kind = default,
                object? value = null,
                [CallerMemberName] string callerMemberName = "",
                [CallerFilePath] string callerFilePath = "",
                [CallerLineNumber] int callerLineNumber = 0,
                params UtdObject[] props
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
            this.PropsByCallerMemberName = new Dictionary<string, UtdObject>();
            if (string.IsNullOrEmpty(this.Name)) { this.Name = this.GetType().FullName!; }
            this.Kind = UpToDocu.Spec.SpecificationCommon.Instance;
        }

        public UpToDocu.Spec.SpecificationCommon SpecificationCommon = UpToDocu.Spec.SpecificationCommon.Instance;

        /*
        public UTDObject Register(
            UTDObject value
            ) {
            this.Props.Add(value);
            return value;
        }
        */

        public T Define<T>(
            Func<T> fnValue,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            )
            where T : UtdObject {
            if (this.PropsByCallerMemberName.TryGetValue(callerMemberName, out var value)) {
                return (T)value;
            } else {
                var reference = new UtdObject(name: "", kind: "reference");
                this.PropsByCallerMemberName.Add(callerMemberName, reference);
                value = fnValue();
                this.Props.Add(value);
                this.PropsByCallerMemberName[callerMemberName] = value;
                reference.Value = value;
                reference.ReferencedValue = value;
                if (reference.Props.Count > 0) {
                    value.AddRange(reference);
                    reference.Props.Clear();
                }
                return (T)value;
            }
            //T? value = default;
            //return this.Define<T>(ref value, fnValue, callerMemberName, callerFilePath, callerLineNumber);
        }

        public T Define<T>(
            ref T? valueT,
            Func<T> fnValue,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
        )
        where T : UtdObject {
            if (valueT is not null) {
                return valueT;
            }
            if (this.PropsByCallerMemberName.TryGetValue(callerMemberName, out var value)) {
                return (T)value;
            } else {
                var reference = new UtdObject(name: "", kind: "reference");
                this.PropsByCallerMemberName.Add(callerMemberName, reference);
                value = valueT = fnValue();
                this.Props.Add(value);
                this.PropsByCallerMemberName[callerMemberName] = value;
                reference.Value = value;
                reference.ReferencedValue = value;
                if (reference.Props.Count > 0) {
                    value.AddRange(reference);
                    reference.Props.Clear();
                }
                return (T)value;
            }
        }

        public void Postpare() {
            foreach (var kv in this.PropsByCallerMemberName) {
                if (string.IsNullOrEmpty(kv.Value.Name)) {
                    kv.Value.Name = kv.Key;
                }
            }
            foreach (var property in this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)) {
                if (typeof(UpToDocu.UtdObject).Equals(property.DeclaringType)) {
                    continue;
                }
                if (property.CanRead) {
                    if (property.PropertyType.IsAssignableTo(typeof(UtdObject))) {
                        if (property.GetValue(this) is UtdObject utdObject) {
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
                if (typeof(UpToDocu.UtdObject).Equals(method.DeclaringType)) {
                    continue;
                }
                if (typeof(UpToDocu.UtdObject).IsAssignableFrom(method.ReturnType)) {
                    if (method.GetParameters().Length == 0) {
                        var methodResult = method.Invoke(obj: this, parameters: Array.Empty<object>());
                        if (methodResult is UtdObject utdObject) {
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