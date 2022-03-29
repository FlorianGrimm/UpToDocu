using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UpToDocu {
    public class UTDObject : IEnumerable<UTDObject> {
        public UTDObject(
            string name = "",
            UTDObject? kind = default,
            object? value = default,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0,
            params UTDObject[] props
            ) {
            this.Props = new System.Collections.Generic.List<UTDObject>();
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

        public UTDObject? CreationAlias { get; set; }

        public List<UTDObject> Props { get; }
        public string Name { get; set; }
        public UTDObject? Kind { get; set; }
        public object? Value { get; set; }
        public string CallerMemberName { get; set; }
        public string CallerFilePath { get; set; }
        public int CallerLineNumber { get; set; }


        public void AddRange(IEnumerable<UTDObject> props) {
            foreach (var prop in props) {
                this.Add(prop);
            }
        }

        public void Add(UTDObject prop) {
            if (this.Props.Contains(prop)) {
            } else {
                this.Props.Add(prop);
            }
        }

        public UTDEnumerator<UTDObject> GetEnumerator() => new UTDEnumerator<UTDObject>(this.Props);

        IEnumerator<UTDObject> IEnumerable<UTDObject>.GetEnumerator() => new UTDEnumerator<UTDObject>(this.Props);

        IEnumerator IEnumerable.GetEnumerator() => new UTDEnumerator<UTDObject>(this.Props);

        public static implicit operator UTDObject(string value) => new UTDObject(value: value);

        public static UTDObject operator +(UTDObject that, UTDObject child) {
            that.Props.Add(child);
            return that;
        }
        public static UTDObject operator /(UTDObject that, UTDObject child) {
            that.Props.Add(child);
            return child;
        }

        /*
 public static Fraction operator +(Fraction a, Fraction b)
=> new Fraction(a.num * b.den + b.num * a.den, a.den * b.den);
*/
    }

    public class UTDEnumerator<T> : IEnumerator<T> where T : UTDObject {
        private readonly List<UTDObject> _List;
        private int _Pos;

        public UTDEnumerator(List<UTDObject> list) {
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