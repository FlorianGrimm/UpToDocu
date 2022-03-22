import { DSValueStore } from "./DSValueStore";
import { DSStateValue } from "./DSStateValue";
export class DSArrayStore extends DSValueStore {
    // dirtyEntities: { stateValue: IDSStateValue<Value>, properties?: Set<keyof Value> }[];
    // isProcessDirtyEntityConfigured: boolean;
    constructor(storeName, configuration) {
        super(storeName, configuration);
        this.entities = [];
        // this.dirtyEntities = []
        // this.isProcessDirtyEntityConfigured = false;
    }
    getEntities() {
        return this.entities.map((e, index) => ({ key: index, stateValue: e }));
    }
    create(value) {
        const create = this.configuration.create;
        if (create !== undefined) {
            const result = create(value);
            this.attach(result);
            return result;
        }
        else {
            const result = new DSStateValue(value);
            this.attach(result);
            return result;
        }
    }
    // public initializeRegisteredEvents(): void {
    //     this.isProcessDirtyEntityConfigured = this.hasProcessDirtyEntityConfigured();
    //     this.isProcessDirtyConfigured = this.isProcessDirtyEntityConfigured || this.hasProcessDirtyConfigured();
    // }
    // public hasProcessDirty(): boolean {
    //     if (this.configuration.processDirty !== undefined) { return true; }
    //     if (!(this.processDirty === DSArrayStore.prototype.processDirty)) { return true; }
    //     return false;
    // }
    // public hasProcessDirtyEntityConfigured(): boolean {
    //     if ((this.configuration as ConfigurationDSArrayValueStore<Value>).processDirtyEntity !== undefined) { return true; }
    //     if (!(this.processDirtyEntity === DSArrayStore.prototype.processDirtyEntity)) { return true; }
    //     return false;
    // }
    attach(stateValue) {
        if (stateValue.setStore(this)) {
            this.entities.push(stateValue);
            const index = this.entities.length - 1;
            this.emitEvent("attach", { entity: stateValue, index: index });
        }
    }
    detach(stateValue) {
        const index = this.entities.findIndex((item) => item === stateValue);
        if (index < 0) {
            // do nothing
        }
        else {
            const oldValue = this.entities.splice(index, 1)[0];
            oldValue.store = undefined;
            this.emitEvent("detach", { entity: oldValue, index: index });
        }
    }
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
