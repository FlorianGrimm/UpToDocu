import { DSUIStateValue } from "./DSUIStateValue";
import { dsLog } from "./DSLog";
export class DSStateValue {
    constructor(value) {
        this._value = value;
        this.store = undefined;
        this._stateVersion = 1;
        this.uiStateValue = undefined;
    }
    get stateVersion() {
        return this._stateVersion;
    }
    set stateVersion(value) {
        this._stateVersion = value;
        //dsLog.infoACME("DS", "DSStateValue", "stateVersion", this._value);
    }
    get value() {
        return this._value;
    }
    set value(v) {
        this._value = v;
        this.valueChanged("value", undefined);
    }
    valueChanged(msg, properties) {
        if (this.store !== undefined) {
            this.stateVersion = this.store.getNextStateVersion(this.stateVersion);
            this.store.emitValueChanged(msg ?? "valueChanged", this, properties);
            this.store.emitEvent("value", { entity: this, properties: properties });
        }
        if (this.uiStateValue !== undefined) {
            if (this.store === undefined) {
                dsLog.debugACME("DS", "DSStateValue", "valueChanged", "store is undefined");
                this.uiStateValue.triggerUIUpdate(this.stateVersion);
            }
            else {
                this.store.emitUIUpdate(this.uiStateValue);
            }
        }
    }
    getUIStateValue() {
        if (this.uiStateValue === undefined) {
            return this.uiStateValue = new DSUIStateValue(this);
        }
        else {
            return this.uiStateValue;
        }
    }
    setStore(store) {
        if (this.store === undefined) {
            this.store = store;
            this.stateVersion = store.getNextStateVersion(this.stateVersion);
            return true;
        }
        else if (this.store === store) {
            // ignore
            return false;
        }
        else {
            throw new Error("store already set.");
        }
    }
    getViewProps() {
        return this.getUIStateValue().getViewProps();
    }
    emitUIUpdate() {
        if (this.uiStateValue !== undefined) {
            if (this.store === undefined) {
                this.uiStateValue.triggerUIUpdate(this.stateVersion);
            }
            else {
                this.store.emitUIUpdate(this.uiStateValue);
            }
        }
    }
    triggerUIUpdate(stateVersion) {
        if (this.uiStateValue === undefined) {
            // ignore
        }
        else {
            return this.uiStateValue.triggerUIUpdate(stateVersion);
        }
    }
    get triggerScheduled() {
        if (this.uiStateValue === undefined) {
            return false;
        }
        else {
            return this.uiStateValue.triggerScheduled;
        }
    }
    set triggerScheduled(value) {
        if (this.uiStateValue === undefined) {
        }
        else {
            this.uiStateValue.triggerScheduled = value;
        }
    }
}
export function stateValue(value) {
    return new DSStateValue(value);
}
