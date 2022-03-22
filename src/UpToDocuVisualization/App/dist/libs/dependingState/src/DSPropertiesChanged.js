import { dsLog } from ".";
const cache = [];
export function getPropertiesSet(keys) {
    return new Set(keys);
}
export function getPropertiesChanged(that) {
    const result = cache.pop();
    if (result === undefined) {
        return new DSPropertiesChanged(that);
    }
    else {
        result.instance = that;
        return result;
    }
}
export class DSPropertiesChanged {
    constructor(instance) {
        this.instance = instance;
        this.properties = new Set();
    }
    add(key) {
        this.properties.add(key);
    }
    setIf(key, value, fnIsEqual) {
        const isEqual = ((fnIsEqual === undefined)
            ? (this.instance.value[key] === value)
            : (fnIsEqual(this.instance.value[key], value)));
        if (isEqual) {
            // skip
            return false;
        }
        else {
            this.instance.value[key] = value;
            this.add(key);
            return true;
        }
    }
    get hasChanged() {
        return this.properties.size > 0;
    }
    giveBack() {
        this.instance = null;
        this.properties.clear();
        cache.push(this);
    }
    valueChangedIfNeeded(msg) {
        if (this.properties.size === 0) {
            this.instance = null;
            cache.push(this);
            return false;
        }
        else {
            const instance = this.instance;
            this.instance = null;
            if (dsLog.isEnabled("valueChangedIfNeeded")) {
                dsLog.infoACME("DS", "DSPropertiesChanged", "valueChangedIfNeeded", msg);
            }
            instance.valueChanged(msg, this.properties);
            return true;
        }
    }
    toString() {
        return Array.from(this.properties.keys()).join(", ");
    }
}
export function hasChangedProperty(properties, property1, property2, property3, property4) {
    return ((properties === undefined)
        || ((property1 === undefined) || (properties.has(property1)))
        || ((property2 === undefined) || (properties.has(property2)))
        || ((property3 === undefined) || (properties.has(property3)))
        || ((property4 === undefined) || (properties.has(property4))));
}
//
