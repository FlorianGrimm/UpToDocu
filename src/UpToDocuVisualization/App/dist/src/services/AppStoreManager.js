import { DSStoreManager } from "dependingState";
export class AppStoreManager extends DSStoreManager {
    constructor(routerStore, navigatorStore, appUIStore, pageAStore, pageBStore) {
        super();
        this.routerStore = routerStore;
        this.navigatorStore = navigatorStore;
        this.appUIStore = appUIStore;
        this.pageAStore = pageAStore;
        this.pageBStore = pageBStore;
        this.attach(routerStore);
        this.attach(navigatorStore);
        this.attach(appUIStore);
        this.attach(pageAStore);
        this.attach(pageBStore);
    }
}
