import {
    IDSObjectStateValue,
    DSUIProps,
    IDSValueStoreWithValue,
    IDSStateCollectionValue
} from "./types";
import { DSUIStateValue } from "./DSUIStateValue";
import { dsLog } from "./DSLog";
import { DSEventEntityVSValue } from "./types";

/*
export class DSStoreStateValue<Value> implements IDSStateValue<Value>{
    store: IDSValueStoreWithValue<Value> | undefined;
    stateVersion: number;
    value: Value;
    triggerScheduled: boolean;
    constructor() {
        this.stateVersion = 0;
        this.value = undefined! as any;
        this.triggerScheduled = false;
    }
    valueChanged(msg: string, properties?: Set<keyof Value>): void {
        throw new Error("Method not implemented.");
    }
    getUIStateValue(): DSUIStateValue<Value> {
        throw new Error("Method not implemented.");
    }
    setStore(store: IDSValueStoreWithValue<Value>): boolean {
        throw new Error("Method not implemented.");
    }
    getViewProps(): DSUIProps<Value> {
        throw new Error("Method not implemented.");
    }
    emitUIUpdate(): void {
        throw new Error("Method not implemented.");
    }
    triggerUIUpdate(stateVersion: number): void {
        throw new Error("Method not implemented.");
    }
}
*/

export class DSCollectionStoreStateValue<
    Value
    > implements IDSStateCollectionValue<Value>
    {
    //store: IDSValueStoreWithValue<Value[]> | undefined;
    store: IDSValueStoreWithValue<Value> | undefined;
    //stateVersion: number;
    uiStateValue: DSUIStateValue<Value> | undefined;

    _getValue: () => Value[];
    _setValue: ((value: Value[]) => void) | undefined;

    constructor(
        store: IDSValueStoreWithValue<Value> | undefined,
        getValue: ()=>Value[],
        setValue: (((value: Value[]) => void) | undefined) = undefined
    ) {
        this.store = store;
        this.uiStateValue = undefined;
        this._getValue = getValue;
        this._setValue = setValue;
    }

    public get value(): Value[] {
        if (this.store !== undefined) {
            return this._getValue();
        } else {
            return [];
        }
    }
    public set value(value: Value[]) {
        if (this.store !== undefined && this._setValue !== undefined) {
            this._setValue(value);
        } else{
            throw new Error("not supported");
        }
    }

    public get stateVersion(): number {
        return (this.store !== undefined) ? this.store.stateVersion : 0;
    }

    public set stateVersion(value: number) {
    }

    valueChanged(msg: string, properties?: Set<keyof Value[]>): void {
        throw new Error("Method not implemented.");
    }
    getUIStateValue(): DSUIStateValue<Value[]> {
        if (this.uiStateValue === undefined) {
            return this.uiStateValue = new DSUIStateValue<Value[]>(this);
        } else {
            return this.uiStateValue;
        }
    }
    setStore(store: IDSValueStoreWithValue<Value[]>): boolean {
        if (this.store === undefined) {
            this.store = store as Store;
            return true;
        } else if (this.store === store) {
            return false;
        } else {
            throw new Error("store already set.");
        }
    }
    getViewProps(): DSUIProps<Value[]> {
        return this.getUIStateValue().getViewProps();
    }
    emitUIUpdate(): void {
        if (this.uiStateValue !== undefined) {
            if (this.store === undefined) {
                this.uiStateValue.triggerUIUpdate(this.stateVersion);
            } else {
                this.store.emitUIUpdate(this.uiStateValue);
            }
        }
    }
    triggerUIUpdate(stateVersion: number): void {
        if (this.uiStateValue === undefined) {
            // ignore
        } else {
            return this.uiStateValue.triggerUIUpdate(stateVersion);
        }
    }

    public get triggerScheduled(): boolean {
        if (this.uiStateValue === undefined) {
            return false;
        } else {
            return this.uiStateValue.triggerScheduled;
        }
    }

    public set triggerScheduled(value: boolean) {
        if (this.uiStateValue === undefined) {
        } else {
            this.uiStateValue.triggerScheduled = value;
        }
    }
}