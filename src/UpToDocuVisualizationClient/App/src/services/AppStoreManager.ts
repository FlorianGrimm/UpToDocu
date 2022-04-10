import {
    IDSStoreManager,
    DSStoreManager
} from "dependingState";

import type { DSRouterStore, DSRouterValue } from "dependingStateRouter";
import type { AppUIStore } from "~/components/AppUI/AppUIStore";
import type { NavigatorStore } from "~/components/Navigator/NavigatorStore";
import type { PageHomeStore } from "~/components/PageHome/PageHomeStore";
import type { PageGraphStore } from "~/components/PageGraph/PageGraphStore";
import type { GraphNodeStore } from '~/components/GraphNode/GraphNodeStore';
import type { GraphLinkStore } from '~/components/GraphLink/GraphLinkStore';
import { GraphNavigatorStore } from "~/components/GraphNavigator/GraphNavigatorStore";

export interface IAppStoreManager extends IDSStoreManager {
    routerStore: DSRouterStore<DSRouterValue>;
    navigatorStore: NavigatorStore;
    appUIStore: AppUIStore;
    pageHomeStore: PageHomeStore;
    pageGraphStore: PageGraphStore;
    graphNavigatorStore : GraphNavigatorStore;
    graphNodeStore: GraphNodeStore;
    graphLinkStore: GraphLinkStore;
}

export class AppStoreManager extends DSStoreManager implements IAppStoreManager {
    constructor(
        public routerStore: DSRouterStore<DSRouterValue>,
        public navigatorStore: NavigatorStore,
        public appUIStore: AppUIStore,
        public pageHomeStore: PageHomeStore,
        public pageGraphStore: PageGraphStore,
        public graphNavigatorStore : GraphNavigatorStore ,
        public graphNodeStore: GraphNodeStore,
        public graphLinkStore: GraphLinkStore,
    ) {
        super();
        this.attach(routerStore);
        this.attach(navigatorStore);
        this.attach(appUIStore);
        this.attach(pageHomeStore);
        this.attach(pageGraphStore);
        this.attach(graphNodeStore);
        this.attach(graphLinkStore);
    }
}