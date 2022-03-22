import { DSMapStore } from "./DSMapStore";
import { DSStateValue } from "./DSStateValue";
export class DSEntityStore extends DSMapStore {
    constructor(storeName, configuration) {
        super(storeName, configuration);
    }
    set(value) {
        const getKey = this.configuration.getKey;
        const create = this.configuration.create;
        if (create !== undefined) {
            const result = create(value);
            const key = getKey(value);
            this.attach(key, result);
            return result;
        }
        else {
            const result = new DSStateValue(value);
            const key = getKey(value);
            this.attach(key, result);
            return result;
        }
    }
    setMany(values) {
        values.forEach(this.set, this);
    }
}
