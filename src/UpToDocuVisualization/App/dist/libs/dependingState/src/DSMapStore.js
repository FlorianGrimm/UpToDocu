import { DSValueStore } from "./DSValueStore";
import { DSStateValue } from "./DSStateValue";
export class DSMapStore extends DSValueStore {
    // dirtyEntities: { stateValue: IDSStateValue<Value>, properties?: Set<keyof Value> }[];
    // isProcessDirtyEntityConfigured: boolean;
    constructor(storeName, configuration) {
        super(storeName, configuration);
        this.entities = new Map();
        // this.dirtyEntities = [];
        // this.isProcessDirtyEntityConfigured = false;
    }
    create(key, value) {
        const create = this.configuration.create;
        if (create !== undefined) {
            const result = create(value);
            this.attach(key, result);
            return result;
        }
        else {
            const result = new DSStateValue(value);
            this.attach(key, result);
            return result;
        }
    }
    get(key) {
        return this.entities.get(key);
    }
    getEntities() {
        return Array.from(this.entities.entries()).map((e) => ({ key: e[0], stateValue: e[1] }));
    }
    // public initializeRegisteredEvents(): void {
    //     this.isProcessDirtyEntityConfigured = this.hasProcessDirtyEntityConfigured();
    //     this.isProcessDirtyConfigured = this.isProcessDirtyEntityConfigured || this.hasProcessDirtyConfigured();
    // }
    // public hasProcessDirty(): boolean {
    //     if (this.configuration.processDirty !== undefined) { return true; }
    //     if (!(this.processDirty === DSMapStore.prototype.processDirty)) { return true; }
    //     return false;
    // }
    // public hasProcessDirtyEntityConfigured(): boolean {
    //     if ((this.configuration as ConfigurationDSMapValueStore<Value>).processDirtyEntity !== undefined) { return true; }
    //     if (!(this.processDirtyEntity === DSMapStore.prototype.processDirtyEntity)) { return true; }
    //     return false;
    // }
    attach(key, stateValue) {
        if (stateValue.setStore(this)) {
            const oldValue = this.entities.get(key);
            if (oldValue === undefined) {
                this.entities.set(key, stateValue);
                this.emitEvent("attach", { entity: stateValue, key: key });
            }
            else if (oldValue === stateValue) {
                // do nothing
            }
            else {
                oldValue.store = undefined;
                this.emitEvent("detach", { entity: oldValue, key: key });
                this.entities.set(key, stateValue);
                this.emitEvent("attach", { entity: stateValue, key: key });
            }
            return oldValue;
        }
        else {
            return stateValue;
        }
    }
    detach(key) {
        const oldValue = this.entities.get(key);
        if (oldValue === undefined) {
            // do nothing
        }
        else {
            oldValue.store = undefined;
            this.emitEvent("detach", { entity: oldValue, key: key });
        }
    }
    // public emitValueChanged(msg: string, stateValue?: IDSStateValue<Value>, properties?: Set<keyof Value>): void {
    //     super.emitValueChanged(msg, stateValue, properties);
    //     if (stateValue !== undefined) {
    //         this.dirtyEntities.push({ stateValue, properties });
    //     }
    // }
    listenEventAttach(msg, callback) {
        return this.listenEvent(msg, "attach", callback);
    }
    listenEventValue(msg, callback) {
        return this.listenEvent(msg, "value", callback);
    }
    listenEventDetach(msg, callback) {
        return this.listenEvent(msg, "detach", callback);
    }
}
