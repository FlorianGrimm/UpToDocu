import { storeBuilder } from "dependingState";

// the import type prevents that webpack add a depenency
import type { GraphNavigatorStore } from "./GraphNavigatorStore";

export const graphNavigatorStoreBuilder = storeBuilder<GraphNavigatorStore['storeName']>("GraphNavigatorStore");
// export const countDown = graphNavigatorStoreBuilder.createAction<undefined>("countDown");
// export const countUp = graphNavigatorStoreBuilder.createAction<undefined>("countUp");

/*
copy this to index.ts - main() - // create all stores


graphNavigatorStore,

copy this to AppStoreManager.ts

import type { GraphNavigatorStore } from "~/components/GraphNavigator/GraphNavigatorStore";
graphNavigatorStore: GraphNavigatorStore;
public graphNavigatorStore: GraphNavigatorStore,
        this.attach(graphNavigatorStore);
*/