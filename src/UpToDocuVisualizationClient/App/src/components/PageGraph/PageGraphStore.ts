import {
    DSObjectStore,
} from "dependingState";
import type { IAppStoreManager } from "~/services/AppStoreManager";
import { pageGraphStoreBuilder } from "./PageGraphActions";
import { PageGraphValue } from "./PageGraphValue";


export class PageGraphStore extends DSObjectStore<PageGraphValue, "PageGraphStore"> {

    constructor(value: PageGraphValue|undefined) {
        super("PageGraphStore", value ?? new PageGraphValue());
        pageGraphStoreBuilder.bindValueStore(this);
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