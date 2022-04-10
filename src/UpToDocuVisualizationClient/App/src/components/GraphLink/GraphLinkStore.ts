import {
    DSEntityStore
} from "dependingState";
import type { IAppStoreManager } from "~/services/AppStoreManager";
import { GraphLinkValue } from "./GraphLinkValue";


export class GraphLinkStore extends DSEntityStore<GraphLinkValue, "GraphLinkStore"> {
    appStateStateVersion: number;
    appViewProjectsUIStoreStateVersion: number;

    constructor() {
        super("GraphLinkStore");
        this.appStateStateVersion = 0;
        this.appViewProjectsUIStoreStateVersion = 0;
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