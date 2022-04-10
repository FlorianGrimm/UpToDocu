using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace UpToDocu {
    public enum UtdValueKind {
        Value = 1,
        Failed = 2,
        Error = 3
    }
    public class UtdValue {
        public static UtdValue<T> Value<T>(UtdObject spec, T value) {
            return new UtdValue<T>(spec, value, null, UtdValueKind.Value);
        }

        public static UtdValue Failed(UtdObject spec) {
            return new UtdValue(spec, null, UtdValueKind.Failed);
        }

        public static UtdValue Faulted(UtdObject spec, Exception error) {
            return new UtdValue(spec, error, UtdValueKind.Error);
        }

        public static UtdValue<T> ConditionalValue<T>(UtdObject spec, T value, Func<T, bool> validate) {
            try {
                var result = validate(value);
                return new UtdValue<T>(
                    spec,
                    value,
                    null,
                    result ? UtdValueKind.Value : UtdValueKind.Failed);
            } catch (System.Exception error) {
                return new UtdValue<T>(spec, value, error, UtdValueKind.Error);
            }
        }

        private UtdObject _Spec;
        private Exception? _Error;
        private UtdValueKind _Kind;

        public UtdValue(UtdObject spec, Exception? error, UtdValueKind kind) {
            this._Spec = spec;
            this._Error = error;
            this._Kind = kind;
        }

        public void Deconstruct(out UtdObject spec, out Exception? error, out UtdValueKind kind) {
            spec = this._Spec;
            error = this._Error;
            kind = this._Kind;
        }
    }

    public class UtdValue<T> {
        private UtdObject _Spec;
        private T _Value;
        private Exception? _Error;
        private UtdValueKind _Kind;

        public UtdValue(UtdObject spec, T value, Exception? error, UtdValueKind kind) {
            this._Spec = spec;
            this._Value = value;
            this._Error = error;
            this._Kind = kind;
        }

        public void Deconstruct(out UtdObject spec, out T value, out Exception? error, out UtdValueKind kind) {
            spec = this._Spec;
            value = this._Value;
            error = this._Error;
            kind = this._Kind;
        }

        public bool TryGetValue([MaybeNullWhen(false)] out T value) {
            if (this._Kind == UtdValueKind.Value) {
                value = this._Value;
                return true;
            } else {
                value = default!;
                return false;
            }
        }

        public T GetValue() {
            if (this._Kind == UtdValueKind.Value) {
                return this._Value;
            } else {
                throw new InvalidOperationException();
            }
        }

        public T GetValue(T defaultValue) {
            if (this._Kind == UtdValueKind.Value) {
                return this._Value;
            } else {
                return defaultValue;
            }
        }

        public static implicit operator UtdValue<T>(UtdValue src) {
            var (spec, error, kind) = src;
            return new UtdValue<T>(spec, default(T)!, error, kind);
        }


        public UtdValue<R> ThenValue<R>(
            Func<T, UtdValue<T>, UtdValue<R>> ifValue,
            Func<UtdValue<T>, UtdValue<R>>? ifFailed = default,
            Func<Exception, UtdValue<T>, UtdValue<R>>? ifError = default
            ) {
            if (this._Kind == UtdValueKind.Value) {
                return ifValue(this._Value, this);
            } else if (this._Kind == UtdValueKind.Failed) {
                if (ifFailed is null) {
                    return new UtdValue<R>(this._Spec, default(R)!, null, UtdValueKind.Failed);
                } else {
                    return ifFailed(this);
                }
            } else if (this._Kind == UtdValueKind.Error) {
                if (ifError is null) {
                    return new UtdValue<R>(this._Spec, default(R)!, this._Error, UtdValueKind.Failed);
                } else {
                    return ifError(this._Error ?? new Exception(), this);
                }
            } else {
                return new UtdValue<R>(this._Spec, default(R)!, this._Error ?? new Exception(), UtdValueKind.Error);
            }
        }

        public async Task<UtdValue<R>> ThenValueAsync<R>(
            Func<T, UtdValue<T>, Task<UtdValue<R>>> ifValue,
            Func<UtdValue<T>, UtdValue<R>>? ifFailed = default,
            Func<Exception, UtdValue<T>, UtdValue<R>>? ifError = default
            ) {
            if (this._Kind == UtdValueKind.Value) {
                return await ifValue(this._Value, this);
            } else if (this._Kind == UtdValueKind.Failed) {
                if (ifFailed is null) {
                    return new UtdValue<R>(this._Spec, default(R)!, null, UtdValueKind.Failed);
                } else {
                    return ifFailed(this);
                }
            } else if (this._Kind == UtdValueKind.Error) {
                if (ifError is null) {
                    return new UtdValue<R>(this._Spec, default(R)!, this._Error, UtdValueKind.Failed);
                } else {
                    return ifError(this._Error ?? new Exception(), this);
                }
            } else {
                return new UtdValue<R>(this._Spec, default(R)!, this._Error ?? new Exception(), UtdValueKind.Error);
            }
        }
        //
        public R Result<R>(
            Func<T, UtdValue<T>, R> ifValue,
            Func<UtdValue<T>, R> ifFailed,
            Func<Exception, UtdValue<T>, R> ifError
            ) {
            if (this._Kind == UtdValueKind.Value) {
                return ifValue(this._Value, this);
            } else if (this._Kind == UtdValueKind.Failed) {
                return ifFailed(this);
            } else if (this._Kind == UtdValueKind.Error) {
                return ifError(this._Error ?? new Exception(), this);
            } else {
                throw new Exception();
            }
        }
        public async Task<R> ResultAsync<R>(
            Func<T, UtdValue<T>, Task<R>> ifValue,
            Func<UtdValue<T>, R> ifFailed,
            Func<Exception, UtdValue<T>, R> ifError
            ) {
            if (this._Kind == UtdValueKind.Value) {
                return await ifValue(this._Value, this);
            } else if (this._Kind == UtdValueKind.Failed) {
                return ifFailed(this);
            } else if (this._Kind == UtdValueKind.Error) {
                return ifError(this._Error ?? new Exception(), this);
            } else {
                throw new Exception();
            }
        }
    }
}