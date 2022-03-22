import { DSValueStore } from "./DSValueStore";
export class DSObjectStore extends DSValueStore {
    constructor(storeName, stateValue, configuration) {
        super(storeName, configuration);
        this.stateValue = stateValue;
        stateValue.setStore(this);
    }
    getEntities() {
        return [{ key: "stateValue", stateValue: this.stateValue }];
    }
    listenEventValue(msg, callback) {
        return this.listenEvent(msg, "value", callback);
    }
}
