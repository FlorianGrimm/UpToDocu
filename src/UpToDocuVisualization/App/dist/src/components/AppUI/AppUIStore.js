import { DSObjectStore, hasChangedProperty } from "dependingState";
// import type { IAppStoreManager } from "~/services/AppStoreManager";
import { appUIStoreBuilder, loadData } from "./AppUIActions";
import { getAppStoreManager } from "~/singletonAppStoreManager";
export class AppUIStore extends DSObjectStore {
    constructor(value) {
        super("AppUIStore", value);
        appUIStoreBuilder.bindValueStore(this);
    }
    initializeStore() {
        super.initializeStore();
        loadData.listenEvent("TODO", (e) => {
        });
        const navigatorStore = getAppStoreManager().navigatorStore;
        navigatorStore.listenValueChanged("AppUIStore listen to router", (stateValue, properties) => {
            if (hasChangedProperty(properties, "page")) {
                this.stateValue.emitUIUpdate();
            }
        });
        // this.isDirty=true;
    }
}
