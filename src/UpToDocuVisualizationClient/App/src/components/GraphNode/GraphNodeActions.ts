import { storeBuilder } from "dependingState";

// the import type prevents that webpack add a depenency
import type { GraphNodeStore } from "./GraphNodeStore";

export const graphNodeStoreBuilder = storeBuilder<GraphNodeStore['storeName']>("GraphNodeStore");
// export const countDown = graphNodeStoreBuilder.createAction<undefined>("countDown");
// export const countUp = graphNodeStoreBuilder.createAction<undefined>("countUp");

/*
copy this to index.ts - main() - // create all stores


graphNodeStore,

copy this to AppStoreManager.ts

import type { GraphNodeStore } from "~/components/GraphNode/GraphNodeStore";
graphNodeStore: GraphNodeStore;
public graphNodeStore: GraphNodeStore,
        this.attach(graphNodeStore);
*/