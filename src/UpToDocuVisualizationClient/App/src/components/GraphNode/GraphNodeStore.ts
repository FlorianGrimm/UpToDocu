import {    
    DSEntityStore, 
    DSCollectionStoreStateValue
} from "dependingState";
import type { IAppStoreManager } from "~/services/AppStoreManager";
import { GraphNodeValue } from "./GraphNodeValue";


export class GraphNodeStore extends DSEntityStore<string, GraphNodeValue, "GraphNodeStore"> {
    readonly stateValue: DSCollectionStoreStateValue<GraphNodeValue>;
    
    constructor() {
        super("GraphNodeStore");
        this.stateValue = new DSCollectionStoreStateValue<GraphNodeValue>(
            this,
            ()=> Array.from(this.entities.values())
            );
    }

    public initializeStore(): void {
        super.initializeStore();

        //const appState = (this.storeManager! as unknown as IAppStoreManager).appStore;
        //appState.listenDirtyRelated(this.storeName, this);
    }

    // public processDirty(): boolean {
    //     let result=super.processDirty();
    //     return  result;
    // }

}