using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace UpToDocu {
    public static class UTD {
        public static UTDObject UTDObject(
            string name = "",
            UTDObject? kind = default,
            object? value = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            return new UTDObject(
                name: name,
                kind: kind,
                value: value,
                callerMemberName: callerMemberName,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber);
        }

        public static UTDObject UTDClass<T>(
            string? name = null,
            object? value = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            //UTDMethod
            return new UTDObject(
                name: name ?? typeof(T).FullName ?? string.Empty,
                kind: global::UpToDocu.Spec.SpecificationCommon.Instance.Class,
                value: value ?? typeof(T),
                callerMemberName: callerMemberName,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber);
        }
        public static UTDObject UTDMethod(
            string name = "",
            object? value = null,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0
            ) {
            return new UTDObject(
                name: name,
                kind: global::UpToDocu.Spec.SpecificationCommon.Instance.Method,
                value: value,
                callerMemberName: callerMemberName,
                callerFilePath: callerFilePath,
                callerLineNumber: callerLineNumber);
        }


        public static bool Condition<T>(
            UTDBoundObject utd,
            T value,
            Func<T, bool> condition
            ) {
            return false;
        }


        public static R Conditional<T, R>(
            UTDBoundObject utd,
            T value,
            Func<T, bool> condition,
            Func<T, R> ifTrue,
            Func<T, R> ifFalse
            ) {
            if (condition(value)) {
                return ifTrue(value);
            } else {
                return ifFalse(value);
            }
        }

        public static async Task<R> ConditionalAsync<T, R>(
            UTDBoundObject utd,
            T value,
            Func<T, bool> condition,
            Func<T, Task<R>> ifTrue,
            Func<T, Task<R>> ifFalse
            ) {
            if (condition(value)) {
                var result = await ifTrue(value);
                return result;
            } else {
                var result = await ifFalse(value);
                return result;
            }
        }

        public static async Task<R> ConditionalAsync<T, R>(
            UTDBoundObject utd,
            T value,
            Func<T, bool> condition,
            Func<T, Task<R>> ifTrue,
            Func<T, R> ifFalse
            ) {
            if (condition(value)) {
                var result = await ifTrue(value);
                return result;
            } else {
                var result = ifFalse(value);
                return result;
            }
        }
    }
}