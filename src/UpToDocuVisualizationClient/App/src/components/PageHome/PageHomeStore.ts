import {
    DSObjectStore,
} from "dependingState";
import type { IAppStoreManager } from "~/services/AppStoreManager";
import { pageHomeStoreBuilder } from "./PageHomeActions";
import { PageHomeValue } from "./PageHomeValue";


export class PageHomeStore extends DSObjectStore<PageHomeValue, "PageHomeStore"> {

    constructor(value: PageHomeValue | undefined) {
        super("PageHomeStore", value ?? new PageHomeValue());
        pageHomeStoreBuilder.bindValueStore(this);
    }

    public initializeStore(): void {
        super.initializeStore();

        //const xxxStore = (this.storeManager! as IAppStoreManager).xxxStore;
        //xxxStore.listenDirtyRelated(this.storeName, this);      

        // countDown.listenEvent("TODO", (e) => {
        // });

        // countUp.listenEvent("TODO", (e) => {
        // });
    }

    // public processDirty(): boolean {
    //     let result=super.processDirty();
    //     return  result;
    // }

}