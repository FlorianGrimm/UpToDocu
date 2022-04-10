using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UpToDocu {
    public class UtdObject : IEnumerable<UtdObject> {
        public UtdObject(
            string name = "",
            UtdObject? kind = default,
            object? value = default,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0,
            params UtdObject[] props
            ) {
            this.Props = new System.Collections.Generic.List<UtdObject>();
            this.Name = name;
            this.Kind = kind;
            this.Value = value;
            this.CallerMemberName = callerMemberName;
            this.CallerFilePath = callerFilePath;
            this.CallerLineNumber = callerLineNumber;
            if ((props is not null) && (props.Length > 0)) {
                this.Props.AddRange(props);
            }
        }

        public UtdObject? CreationAlias { get; set; }
        public UtdObject? ReferencedValue { get; set; }

        public List<UtdObject> Props { get; }
        public string Name { get; set; }
        public UtdObject? Kind { get; set; }
        public object? Value { get; set; }
        public string CallerMemberName { get; set; }
        public string CallerFilePath { get; set; }
        public int CallerLineNumber { get; set; }


        public void AddRange(IEnumerable<UtdObject> props) {
            foreach (var prop in props) {
                this.Add(prop);
            }
        }

        public void Add(UtdObject prop) {
            if (this.Props.Contains(prop)) {
            } else {
                this.Props.Add(prop);
            }
        }

        public UTDEnumerator<UtdObject> GetEnumerator() => new UTDEnumerator<UtdObject>(this.Props);

        IEnumerator<UtdObject> IEnumerable<UtdObject>.GetEnumerator() => new UTDEnumerator<UtdObject>(this.Props);

        IEnumerator IEnumerable.GetEnumerator() => new UTDEnumerator<UtdObject>(this.Props);

        public static implicit operator UtdObject(string value) => new UtdObject(value: value);

        public static UtdObject operator +(UtdObject that, UtdObject child) {
            that.Props.Add(child);
            return that;
        }
        public static UtdObject operator /(UtdObject that, UtdObject child) {
            that.Props.Add(child);
            return child;
        }

        /*
 public static Fraction operator +(Fraction a, Fraction b)
=> new Fraction(a.num * b.den + b.num * a.den, a.den * b.den);
*/
    }

    public class UTDEnumerator<T> : IEnumerator<T> where T : UtdObject {
        private readonly List<UtdObject> _List;
        private int _Pos;

        public UTDEnumerator(List<UtdObject> list) {
            this._List = list;
            this._Pos = -1;
        }

        public T Current {
            get {
                if ((0 <= this._Pos) && (this._Pos < this._List.Count)) {
                    return (T)this._List[this._Pos];
                } else {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current => this.Current;

        public bool MoveNext() {
            this._Pos++;
            return this._Pos < this._List.Count;
        }

        public void Dispose() {
        }

        void IEnumerator.Reset() {
            this._Pos = -1;
        }
    }
}