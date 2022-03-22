import { dsLog, } from "./DSLog";
export function storeBuilder(storeName) {
    return new DSStoreBuilder(storeName);
}
export class DSStoreBuilder {
    constructor(storeName) {
        this.storeName = storeName;
        this.actions = new Map();
        this.valueStore = undefined;
    }
    createAction(event) {
        const result = new DSStoreAction(event, this.storeName);
        const key = `${this.storeName}/${event}`;
        if (this.actions.has(key)) {
            throw new Error(`DS createAction event with that name already created. ${key}.`);
        }
        this.actions.set(key, result);
        if (this.valueStore !== undefined) {
            result.bindValueStore(this.valueStore);
        }
        return result;
    }
    bindValueStore(valueStore) {
        this.valueStore = valueStore;
        for (const action of this.actions.values()) {
            dsLog.debugACME("DS", "DSStoreBuilder", "bindValueStore", `${action.storeName}/${action.event}`);
            action.bindValueStore(valueStore);
        }
    }
}
export class DSStoreAction {
    constructor(event, storeName) {
        this.event = event;
        this.storeName = storeName;
    }
    // TODO would it be better to create a DSBoundStoreAction?
    bindValueStore(valueStore) {
        if (this.storeName !== valueStore.storeName) {
            throw new Error("wrong IDSValueStore");
        }
        this.valueStore = valueStore;
    }
    /**
     * add the callback to the event. if the event is emitted (emitEvent) all callback are invoked.
     * @param msg this message is shown in the console
     * @param callback this function is called
     * @returns a function that removes the event
     * @throws throw an Error if the store-constructor doesn't call theStoresBuilder.bindValueStore(this)
     */
    listenEvent(msg, callback) {
        if (this.valueStore === undefined) {
            throw new Error(`DS DSStoreAction.listenEvent valueStore is not set ${this.storeName} - Did you call theStore's-Builder.bindValueStore(this) in the constructor?`);
        }
        else {
            if (!msg) {
                msg = `${this.storeName}/${this.event}`;
            }
            return this.valueStore.listenEvent(msg, this.event, callback);
        }
    }
    /**
     * emit the event
     * @param payload the payload
     */
    emitEvent(payload) {
        if (this.valueStore === undefined) {
            throw new Error(`DS DSStoreAction.emitEvent valueStore is not set ${this.storeName} - Did you call theStore's-Builder.bindValueStore(this) in the constructor?`);
        }
        else {
            this.valueStore.emitEvent(this.event, payload);
        }
    }
    /**
    * emit the event - if needed process will be called
    * @param msg
    * @param payload the payload
    */
    emitEventAndProcess(msg, payload) {
        const valueStore = this.valueStore;
        const storeManager = this.valueStore?.storeManager;
        if ((valueStore === undefined) || (storeManager === undefined)) {
            throw new Error(`DS DSStoreAction.emitEvent valueStore is not set ${this.storeName} - Did you call theStore's-Builder.bindValueStore(this) in the constructor?`);
        }
        else {
            if (storeManager.isProcessing === 0) {
                if (this.valueStore.hasEventHandlersFor(this.event)) {
                    storeManager.process(msg, () => {
                        valueStore.emitEvent(this.event, payload);
                    });
                }
            }
            else {
                valueStore.emitEvent(this.event, payload);
            }
        }
    }
}
