import {
    DSObjectStore,
} from "dependingState";

//import type { IAppStoreManager } from "~/services/AppStoreManager";

import { graphNavigatorStoreBuilder } from "./GraphNavigatorActions";
import { GraphNavigatorValue } from "./GraphNavigatorValue";


export class GraphNavigatorStore extends DSObjectStore<GraphNavigatorValue, "GraphNavigatorStore"> {

    constructor(value: GraphNavigatorValue| undefined) {
        super("GraphNavigatorStore", value ?? new GraphNavigatorValue());
        graphNavigatorStoreBuilder.bindValueStore(this);
    }

    public initializeStore(): void {
        super.initializeStore();
        
        //const xxxStore = (this.storeManager! as IAppStoreManager).xxxStore;
        //xxxStore.listenDirtyRelated(this.storeName, this);      

        // countDown.listenEvent("TODO", (e)=>{
        // });

        // countUp.listenEvent("TODO", (e)=>{
        // });
    }

    // public processDirty(): boolean {
    //     let result=super.processDirty();
    //     return  result;
    // }

}