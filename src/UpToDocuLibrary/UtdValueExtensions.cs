using System;

namespace UpToDocu {
    public static class UtdValueExtensions {
        public static UtdValue<T> AsUtdValue<T>(
            this T value,
            UtdObject spec
            ) => new UtdValue<T>(
                spec,
                value,
                null,
                UtdValueKind.Value);

        public static UtdValue<T> AsUtdValueNotNull<T>(
            this T? value,
            UtdObject spec
            ) where T : class
            => new UtdValue<T>(
                spec,
                value!,
                null,
                (value is not null) ? UtdValueKind.Value : UtdValueKind.Failed);

        public static UtdValue<T> AsUtdValueHasValue<T>(
            this Nullable<T> value,
            UtdObject spec
            ) where T : struct
            => new UtdValue<T>(
                spec,
                value.GetValueOrDefault(),
                null,
                (value.HasValue) ? UtdValueKind.Value : UtdValueKind.Failed);
#if false
        public static UtdValue<R> ThenValue<T, R>(
            this UtdValue<T> that,
            Func<T, UtdValue<T>, UtdValue<R>> ifValue,
            Func<UtdValue<T>, UtdValue<R>>? ifFailed = default,
            Func<Exception, UtdValue<T>, UtdValue<R>>? ifError = default
            ) {
            var (specThat, valueThat, errorThat, kindThat) = that;
            if (kindThat == UtdValueKind.Value) {
                return ifValue(valueThat, that);
            } else if (kindThat == UtdValueKind.Failed) {
                if (ifFailed is null) {
                    return new UtdValue<R>(specThat, default(R)!, null, UtdValueKind.Failed);
                } else {
                    return ifFailed(that);
                }
            } else if (kindThat == UtdValueKind.Error) {
                if (ifError is null) {
                    return new UtdValue<R>(specThat, default(R)!, errorThat, UtdValueKind.Failed);
                } else {
                    return ifError(errorThat ?? new Exception(), that);
                }
            } else {
                return new UtdValue<R>(specThat, default(R)!, errorThat ?? new Exception(), UtdValueKind.Error);
            }
        }

        public static async Task<UtdValue<R>> ThenValueAsync<T, R>(
            this UtdValue<T> that,
            Func<T, UtdValue<T>, Task<UtdValue<R>>> ifValue,
            Func<UtdValue<T>, UtdValue<R>>? ifFailed = default,
            Func<Exception, UtdValue<T>, UtdValue<R>>? ifError = default
            ) {
            var (specThat, valueThat, errorThat, kindThat) = that;
            if (kindThat == UtdValueKind.Value) {
                return await ifValue(valueThat, that);
            } else if (kindThat == UtdValueKind.Failed) {
                if (ifFailed is null) {
                    return new UtdValue<R>(specThat, default(R)!, null, UtdValueKind.Failed);
                } else {
                    return ifFailed(that);
                }
            } else if (kindThat == UtdValueKind.Error) {
                if (ifError is null) {
                    return new UtdValue<R>(specThat, default(R)!, errorThat, UtdValueKind.Failed);
                } else {
                    return ifError(errorThat ?? new Exception(), that);
                }
            } else {
                return new UtdValue<R>(specThat, default(R)!, errorThat ?? new Exception(), UtdValueKind.Error);
            }
        }
        //

        public static R Result<T, R>(
            this UtdValue<T> that,
            Func<T, UtdValue<T>, R> ifValue,
            Func<UtdValue<T>, R> ifFailed,
            Func<Exception, UtdValue<T>, R> ifError
            ) {
            var (specThat, valueThat, errorThat, kindThat) = that;
            if (kindThat == UtdValueKind.Value) {
                return ifValue(valueThat, that);
            } else if (kindThat == UtdValueKind.Failed) {
                return ifFailed(that);
            } else if (kindThat == UtdValueKind.Error) {
                return ifError(errorThat ?? new Exception(), that);
            } else {
                throw new Exception();
            }
        }
        public static async Task<R> ResultAsync<T, R>(
            this UtdValue<T> that,
            Func<T, UtdValue<T>, Task<R>> ifValue,
            Func<UtdValue<T>, R> ifFailed,
            Func<Exception, UtdValue<T>, R> ifError
            ) {
            var (specThat, valueThat, errorThat, kindThat) = that;
            if (kindThat == UtdValueKind.Value) {
                return await ifValue(valueThat, that);
            } else if (kindThat == UtdValueKind.Failed) {
                return ifFailed(that);
            } else if (kindThat == UtdValueKind.Error) {
                return ifError(errorThat ?? new Exception(), that);
            } else {
                throw new Exception();
            }
        }
#endif

    }
}