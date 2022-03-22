export class DSValueChanged {
    constructor(next, value, fnIsEqual) {
        this.next = next;
        this.value = value;
        this.fnIsEqual = fnIsEqual;
    }
    setValue(value) {
        if (this.value === undefined) {
            // changed 
        }
        else if (this.value === value) {
            // same case T T
            return false;
        }
        else if (this.fnIsEqual !== undefined) {
            if (this.fnIsEqual(this.value, value)) {
                // equal
                return false;
            }
        }
        this.value = value;
        if (this.next !== undefined) {
            this.next(value);
        }
        return true;
    }
}
