import { DSValueStore } from "./DSValueStore";
export class DSLooseStore extends DSValueStore {
    // dirtyEntities: { stateValue: IDSStateValue<Value>, properties?: Set<keyof Value> }[];
    // isProcessDirtyEntityConfigured: boolean;
    constructor(storeName, configuration) {
        super(storeName, configuration);
        // this.dirtyEntities = []
        // this.isProcessDirtyEntityConfigured = false;
    }
}
