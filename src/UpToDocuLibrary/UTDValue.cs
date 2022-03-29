using System;
using System.Diagnostics.CodeAnalysis;

namespace UpToDocu {
    public class UTDValue {
        public static UTDValue<T> Value<T>(UTDObject spec, T value) {
            return new UTDValue<T>(spec, value, null, 1);
        }

        public static UTDValue Failed(UTDObject spec) {
            return new UTDValue(spec, null, 2);
        }

        public static UTDValue Faulted(UTDObject spec, Exception error) {
            return new UTDValue(spec, error, 3);
        }

        private UTDObject _Spec;
        private Exception? _Error;
        private int _Kind;

        public UTDValue(UTDObject spec, Exception? error, int kind) {
            this._Spec = spec;
            this._Error = error;
            this._Kind = kind;
        }

        public void Deconstruct(out UTDObject spec, out Exception? error, out int kind) {
            spec = this._Spec;
            error = this._Error;
            kind = this._Kind;
        }
    }
    public class UTDValue<T> {
        private UTDObject _Spec;
        private T _Value;
        private Exception? _Error;
        private int _Kind;

        public UTDValue(UTDObject spec, T value, Exception? error, int kind) {
            this._Spec = spec;
            this._Value = value;
            this._Error = error;
            this._Kind = kind;
        }

        public bool TryGetValue([MaybeNullWhen(false)] out T value) {
            if (this._Kind == 1) {
                value = this._Value;
                return true;
            } else {
                value = default!;
                return false;
            }
        }

        public static explicit operator UTDValue<T>(UTDValue src) {
            var (spec, error, kind) = src;
            return new UTDValue<T>(spec, default(T)!, error, kind);
        }
    }
}